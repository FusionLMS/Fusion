namespace Fusion.Core.Auth;

public static class AuthDefaults
{
    public static class Roles
    {
        public static readonly RoleDto BackendDeveloper = new()
        {
            Id = "rol_KLDXYwt14TkSw0QS",
            Name = "Backend Developer",
            Description = "DEV environments roles only. Grant access to all functionality and all existing data"
        };

        public static readonly RoleDto Admin = new()
        {
            Id = "rol_KEWxoVUMkIKdt3Bn",
            Name = "Admin",
            Description = "Admins has full access to their organizations"
        };

        public static readonly RoleDto Manager = new()
        {
            Id = "rol_9JuPT7MXrG6DM0TL",
            Name = "Manager",
            Description = "This role allows to create and manage courses. " +
                          "Also managers can approve teachers and has all their permissions"
        };

        public static readonly RoleDto Teacher = new()
        {
            Id = "rol_vHlRTczVsiT2e6lS",
            Name = "Teacher",
            Description = "Allows to create assignment, add students to course and manage them"
        };

        public static readonly RoleDto Student = new()
        {
            Id = "rol_4mPj42McRdKX00XG",
            Name = "Student",
            Description = "Allows to view enrolled courses and work with their assignments"
        };

        public static Dictionary<string, RoleDto> All { get; } = new()
        {
            { "Backend Developer", BackendDeveloper },
            { nameof(Admin), Admin },
            { nameof(Manager), Manager },
            { nameof(Teacher), Teacher },
            { nameof(Student), Student }
        };
    }
}