using Irihi.Dogma.Docs;

namespace Ursa.Demo.Common;

public class UrsaDocSite: DocSite
{
    public static UrsaDocSite Instance { get; } = new();
    private UrsaDocSite()
    {
        
    }
}
