namespace Mitgliederverwaltung.Services
{
    public interface IDemoService
    {
        string PcName { get; }
        string SavedText { get; set; }
        string DefaultPath { get; set; }
    }
}