namespace CareerCompassAPI.Domain.Enums
{
    [Flags]
    public enum Skills
    {
        None = 0,
        Communication = 1,
        Leadership = 2,
        ProblemSolving = 4,
        Teamwork = 8,
        Creativity = 16,
        TechnicalLiteracy = 32,
        ProjectManagement = 64,
        TimeManagement = 128,
        AnalyticalThinking = 256,
        AttentionToDetail = 512
    }
}
