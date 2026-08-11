using Irihi.Dogma.Docs;

namespace Ursa.Demo.Models;

public class UrsaDocSite: DocSite
{
    public static UrsaDocSite Instance { get; } = new();
    private UrsaDocSite()
    {
        
    }
}
