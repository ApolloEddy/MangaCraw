using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aries;
using System.Xml;
using System.Xml.Serialization;

namespace MangaCraw
{
    public class DataCotainer
    {
        public MangaDetail Data { get; set; }
        public readonly string RootDirectory;

        public DataCotainer(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            RootDirectory = dir;
            Data = new MangaDetail();
        }
    }
}


// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class MangaDetail
{

    private string nameField;

    private string authorField;

    private string statusField;

    private MangaDetailChapter[] catalogeField;

    /// <remarks/>
    public string Name
    {
        get
        {
            return this.nameField;
        }
        set
        {
            this.nameField = value;
        }
    }

    /// <remarks/>
    public string Author
    {
        get
        {
            return this.authorField;
        }
        set
        {
            this.authorField = value;
        }
    }

    /// <remarks/>
    public string Status
    {
        get
        {
            return this.statusField;
        }
        set
        {
            this.statusField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlArrayItemAttribute("Chapter", IsNullable = false)]
    public MangaDetailChapter[] Cataloge
    {
        get
        {
            return this.catalogeField;
        }
        set
        {
            this.catalogeField = value;
        }
    }
}

/// <remarks/>
[System.SerializableAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class MangaDetailChapter
{

    private string titleField;

    private string[] pathField;

    /// <remarks/>
    public string Title
    {
        get
        {
            return this.titleField;
        }
        set
        {
            this.titleField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Path")]
    public string[] Path
    {
        get
        {
            return this.pathField;
        }
        set
        {
            this.pathField = value;
        }
    }
}


