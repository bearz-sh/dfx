using System.Text;

namespace Bearz.VirtualTerminal;

public class Style
{
    public Rgb Foreground { get; set; } = Rgb.Default;

    public Rgb Background { get; set; } = Rgb.Default;

    public HashSet<Decoration> Decorations { get; set; } = new();

    public string? Link { get; set; }

    public void Add(Decoration decoration) => this.Decorations.Add(decoration);

    public void Add(params Decoration[] decorations) => this.Decorations.UnionWith(decorations);

    public string EmitStart()
    {
        var sb = new StringBuilder();
        sb.Append("\x1b[");
        if (this.Foreground != Rgb.Default)
        {
            switch(this.Foreground)
            {
                case Rgb.Black:
                    sb.Append("30");
                    break;
            }

            sb.Append("38;5;");
            sb.Append(this.Foreground);
            sb.Append(';');
        }
    }
}