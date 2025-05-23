namespace DatabaseAbstractions.Models.Enums
{
    public enum ResponseType
    {
        Correct,

        EmptyTable,
        TableNotExist,
        EntityNotExist,

        LogicError,

        DatabaseAccessError,
        SqlException,
    }
}
