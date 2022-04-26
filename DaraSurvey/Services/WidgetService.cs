using AutoMapper;
using DaraSurvey.Core;
using DaraSurvey.Core.BaseClasses;
using DaraSurvey.Core.Helpers;
using DaraSurvey.Entities;
using DaraSurvey.WidgetServices.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;

namespace DaraSurvey.WidgetServices
{
    public class WidgetService : IWidgetService
    {
        private readonly DB _db;

        public WidgetService(DB db)
        {
            _db = db;
        }

        // ------------------------

        public ViewModelBase GetViewModel(int id)
        {
            var widget = GetEntity(id);

            return GetWidget(widget.Data);
        }

        // ------------------------

        public Widget GetEntity(int id)
        {
            var widget = _db.Set<Widget>()
                .AsNoTracking()
                .SingleOrDefault(o => o.Id == id);
            if (widget == null)
                throw new ServiceException(HttpStatusCode.NotFound, ServiceExceptionCode.WidgetNotFound);

            return widget;
        }

        // ------------------------

        public ViewModelBase Create(EditModelBase model)
        {
            var widget = new Widget {
                Created = DateTime.UtcNow,
                Data = JsonConvert.SerializeObject(model, JsonSeralizerSetting.SerializationSettings)
            };

            _db.Set<Widget>().Add(widget);

            _db.SaveChanges();

            return GetWidget(widget.Data);
        }

        // ------------------------

        public ViewModelBase Updata(int id, EditModelBase model)
        {
            var widget = GetEntity(id);
            widget.Data = JsonConvert.SerializeObject(model, JsonSeralizerSetting.SerializationSettings);
            widget.Updated = DateTime.UtcNow;

            _db.Set<Widget>().Update(widget);
            _db.SaveChanges();

            return GetWidget(widget.Data);
        }

        // ------------------------

        public void Delete(int id)
        {
            var entity = GetEntity(id);

            entity.Deleted = DateTime.UtcNow;

            _db.Set<Widget>().Update(entity);
            _db.SaveChanges();
        }

        // ------------------------

        public ViewModelBase GetWidget(string widgetData)
        {
            var jToken = ((JToken)JsonConvert.DeserializeObject(widgetData, JsonSeralizerSetting.SerializationSettings));

            var typeFormat = "DaraSurvey.Widgets.{0}.ViewModel";
            var binder = new TypeNameSerializationBinder(typeFormat);

            var typeName = jToken["type"].ToString().UppercaseFirst();

            var type = binder.BindToType(null, typeName);

            if (type == null) return null;

            return (ViewModelBase)jToken.ToObject(type);
        }
    }
}