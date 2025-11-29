namespace DerreksLens.Core.Domain.Enums
{
    public enum UserRole
    {
        Guest = 0,
        User = 1,     // Authenticated, can comment
        Admin = 99    // The Author (You)
    }
}