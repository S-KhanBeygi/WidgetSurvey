namespace DaraSurvey.Core
{
    public enum ServiceExceptionCode
    {
        RequestNotFound = 1,
        CreateUserFailed = 2,
        UpdateUserFailed = 3,
        UpdateUserPasswordFailed = 4,
        DeleteUserFailed = 5,
        RequiresTwoFactor = 6,
        PhoneNumberNotVerified = 7,
        EmailNotVerified = 8,
        IsLockedOut = 9,
        LoginFailed = 10,
        DuplicateUserName = 11,
        InvalidToken = 12,
        InvalidPassword = 13,
        OperationFailed = 14,
        ValidationError = 15,
        SurveyAlreadyPublished = 63,
        SurveyAlreadyExpired = 64,
        SurveyExamAlreadyStarted = 65,
        RequestedSurveyHasNotApproveStatus = 66,
        SurveyHasNotAnyQuestion = 67,
        InvalidRequestedSurvey = 68,
        RequestedSurveyHasNotStartedStatus = 69,
        InvalidResponseQuestionId = 70,
        ExamTimeIsFinished = 71,
        InvalidSurveyResponse = 72,
        AlreadyHasThisSurvey = 73,
        UserSurveyHasNotRequestedStatus = 74,
        SurveyHasActiveUser = 75,
        DeleteSurveyQuestionsFirst = 76,
        QuestionsNotFound = 77,
        WidgetNotFound = 78
    }
}
