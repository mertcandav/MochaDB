namespace MochaDB {
  using System;
  using System.Text.RegularExpressions;

  using MochaDB.engine;

  /// <summary>
  /// Attribute for MochaDB.
  /// </summary>
  public class MochaAttribute {
    #region Fields

    private Regex bannedNamesRegex = new Regex(
@"^(Description)\b");

    private string
        name,
        value;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Create new MochaAttribute.
    /// </summary>
    /// <param name="name">Name of attribute.</param>
    public MochaAttribute(string name) {
      Name=name;
      Value=string.Empty;
    }

    /// <summary>
    /// Create new MochaAttribute.
    /// </summary>
    /// <param name="name">Name of attribute.</param>
    /// <param name="value">Value of attribute.</param>
    public MochaAttribute(string name,string value) :
        this(name) => Value=value;

    #endregion Constructors

    #region Events

    /// <summary>
    /// This happens after name changed.
    /// </summary>
    public event EventHandler<EventArgs> NameChanged;
    protected virtual void OnNameChanged(object sender,EventArgs e) {
      //Invoke.
      NameChanged?.Invoke(sender,e);
    }

    /// <summary>
    /// This happens after value changed.
    /// </summary>
    public event EventHandler<EventArgs> ValueChanged;
    protected virtual void OnValueChanged(object sender,EventArgs e) {
      //Invoke.
      ValueChanged?.Invoke(sender,e);
    }

    #endregion Events

    #region Overrides

    /// <summary>
    /// Returns <see cref="Value"/>.
    /// </summary>
    public override string ToString() =>
      Value;

    #endregion Overrides

    #region Properties

    /// <summary>
    /// Name.
    /// </summary>
    public virtual string Name {
      get => name;
      set {
        value=value.Trim();
        if(value==name)
          return;
        if(string.IsNullOrWhiteSpace(value))
          throw new MochaException("Name is cannot null or whitespace!");

        Engine_NAMES.AttributeCheckThrow(value);

        if(bannedNamesRegex.IsMatch(value))
          throw new MochaException($@"Name is cannot ""{value}""");

        name=value;
        OnNameChanged(this,new EventArgs());
      }
    }

    /// <summary>
    /// Value.
    /// </summary>
    public virtual string Value {
      get => value;
      set {
        if(this.value==value)
          return;

        Engine_VALUES.AttributeCheckThrow(value);
        this.value=value;
        OnValueChanged(this,new EventArgs());
      }
    }

    #endregion Properties
  }
}
