namespace Api.Contracts;

public static class ApiRoutes
{
    private const string Root = "api";

    // Define route for Universities :-
    public static class Universities
    {
        public const string Controller = Root + "/universities";

        public const string Create = "";
        public const string GetPaged = "";
        public const string GetList = "all";
        public const string GetById = "{id}";
        public const string Update = "{id}";
        public const string Delete = "{id}";
    }

    // Define route for Majors :-
    public static class Majors
    {
        public const string Controller = Root + "/majors";
        public const string Create = "";
        public const string GetPaged = "";
        public const string GetList = "all";
        public const string GetByUniversityId = "university/{universityId}";
        public const string GetById = "{id}";
        public const string Update = "{id}";
        public const string Delete = "{id}";
    }

    // Define route for Document Types :-
    public static class DocumentTypes
    {
        public const string Controller = Root + "/document-types";
        public const string Create = "";
        public const string GetPaged = "";
        public const string GetList = "all";
        public const string GetById = "{id}";
        public const string Update = "{id}";
        public const string Delete = "{id}";
    }

    // Define route for Professors :-
    public static class Professors
    {
        public const string Controller = Root + "/professors";
        public const string Create = "";
        public const string GetPaged = "";
        public const string GetList = "all";
        public const string GetById = "{id}";
        public const string Update = "{id}";
        public const string Delete = "{id}";
    }
    // Define route for Courses :-
    public static class Courses
    {
        public const string Controller = Root + "/courses";
        public const string Create = "";
        public const string GetPaged = "";
        public const string GetList = "all";
        public const string GetById = "{id}";
        public const string GetByFilter = "filter"; // api/courses/filter?majorId=1&level=2
        public const string Update = "{id}";
        public const string Delete = "{id}";
    }

    // define route for Resources :-
    public static class Resources
    {
        public const string Controller = Root + "/resources";
        public const string Create = "";
        public const string GetPaged = "";
        public const string GetList = "all";
        public const string GetById = "{id}";
        public const string Delete = "{id}";
        public const string Download = "download/{id}";
    }

    // define route for Authentication :-
    public static class Identity
    {
        public const string Controller = Root + "/auth";
        public const string Register = "register";
        public const string Login = "login";
        public const string Refresh = "refresh";
    }

    // define route for Users :-
    public static class Users
    {
        public const string Controller = Root + "/users";
        public const string GetList = "all";
        public const string GetPaged = "";
        public const string GetById = "{id}";
        public const string GetMe = "me";
        public const string Update = "{id}";
        public const string Delete = "{id}";
        public const string ChangePassword = "change-password";
    }
}