namespace WotanStellar.Command;

public record CommandArguments(bool Confirm)
{
    public static readonly CommandArguments Empty = new(false);
}
