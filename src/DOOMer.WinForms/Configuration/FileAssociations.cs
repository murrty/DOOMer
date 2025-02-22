namespace DOOMer.WinForms;

internal sealed class FileAssociations {
    private readonly string[] WADs = [ "wad", "iwad" ];
    private readonly string[] WADPackages = [ "pk3", "ipk3", "pk7", "ipk7", "p7z", "pkz", "pke" ];
    private readonly string[] Dehacks = [ "deh", "bex" ];

    public static void AddWADsAssociation() { }
    public static void RemoveWADsAssociation() { }
    public static void AddWADPackageAssociation() { }
    public static void RemoveWADPackageAssociation() { }
    public static void AddDehacksAssocation() { }
    public static void RemoveDehacksAssociation() { }
}
