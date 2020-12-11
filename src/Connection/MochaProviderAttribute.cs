namespace MochaDB.Connection {
  using System;
  using System.Text.RegularExpressions;

  /// <summary>
  /// Attributes for MochaProviders.
  /// </summary>
  public class MochaProviderAttribute {
    #region Fields

    internal static Regex booleanAttributesRegex = new Regex(@"
AutoConnect|Readonly|AutoCreate",RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    internal string
        name,
        value;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Create new MochaProviderAttribute.
    /// </summary>
    internal protected MochaProviderAttribute() {
      name = string.Empty;
      value = string.Empty;
    }

    /// <summary>
    /// Create new MochaProviderAttribute.
    /// </summary>
    /// <param name="name">Name of attribute.</param>
    /// <param name="value">Value of attribute.</param>
    public MochaProviderAttribute(string name,string value) {
      this.value = value;
      Name = name;
    }

    #endregion Constructors

    #region Operators

    public static explicit operator string(MochaProviderAttribute value) =>
        value.ToString();

    #endregion Operators

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

    #region Internal Members

    /// <summary>
    /// Check value by name.
    /// </summary>
    internal protected virtual void CheckValue() {
      if(string.IsNullOrWhiteSpace(value)) {
        if(Name.Equals("Path"))
          throw new MochaException("File path cannot be empty!");
        if(booleanAttributesRegex.IsMatch(Name))
          value="False";
      } else {
        if(booleanAttributesRegex.IsMatch(Name) && (!value.Equals("true",StringComparison.OrdinalIgnoreCase) ||
            !value.Equals("false",StringComparison.OrdinalIgnoreCase)))
          value="False";
      }
    }

    #endregion

    #region Members

    /// <summary>
    /// Returns string as in provider.
    /// </summary>
    public virtual string GetProviderString() =>
        $"{Name}={Value};";

    #endregion Members

    #region Overrides

    /// <summary>
    /// Returns result of <see cref="GetProviderString()"/>.
    /// </summary>
    public override string ToString() =>
      GetProviderString();

    #endregion Overrides

    #region Properties

    /// <summary>
    /// Name.
    /// </summary>
    public virtual string Name {
      get => name;
      set {
        if(string.IsNullOrWhiteSpace(value))
          throw new MochaException("Attribute name is can not empty or white space!");

        if(value==name)
          return;

        name=value;
        OnNameChanged(this,new EventArgs());
        CheckValue();
      }
    }

    /// <summary>
    /// Value.
    /// </summary>
    public virtual string Value {
      get => value;
      set {
        this.value=value;
        OnValueChanged(this,new EventArgs());
        CheckValue();
      }
    }

    #endregion Properties
  }
}
