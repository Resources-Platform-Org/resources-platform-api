namespace Api.Contracts;

public static class ApiRoutes
{
    private const string Root = "api";

    // Define route for Universities :-
    public static class Universities
    {
        public const string Controller = Root + "/universities";

        public const string Create = "";
        public const string GetAll = "";
        public const string GetById = "{id}";
        public const string Update = "{id}";
        public const string Delete = "{id}";
    }

    // Define route for Majors :-
    public static class Majors
    {
        public const string Controller = Root + "/majors";
        public const string Create = "";
        public const string GetAll = "";
        public const string GetByUniversityId = "university/{universityId}";
        public const string GetById = "{id}";
        public const string Update = "{id}";
        public const string Delete = "{id}";
    }
}