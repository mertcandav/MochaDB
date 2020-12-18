namespace MochaDB {
  using System;
  using System.Collections.Generic;

  using MochaDB.engine;

  /// <summary>
  /// This is column object for MochaDB.
  /// </summary>
  public class MochaColumn {
    #region Fields

    private MochaDataType dataType;
    internal string name;

    #endregion Fields

    #region Internal Constructors

    /// <summary>
    /// Constructor.
    /// </summary>
    internal protected MochaColumn() =>
      Datas = new MochaColumnDataCollection(MochaDataType.String);

    #endregion Internal Constructors

    #region Constructors

    /// <summary>
    /// Create new MochaColumn.
    /// </summary>
    /// <param name="name">Name.</param>
    public MochaColumn(string name) :
        this() {
      Name = name;
      DataType = MochaDataType.String;
      Description = string.Empty;
    }

    /// <summary>
    /// Create new MochaColumn.
    /// </summary>
    /// <param name="name">Name of column.</param>
    /// <param name="dataType">Data type of column.</param>
    public MochaColumn(string name,MochaDataType dataType) :
        this(name) {
      Name = name;
      DataType = dataType;
    }

    /// <summary>
    /// Create new MochaColumn.
    /// </summary>
    /// <param name="name">Name of column.</param>
    /// <param name="dataType">Data type of column.</param>
    public MochaColumn(string name,MochaDataType dataType,IEnumerable<MochaData> datas) :
        this(name,dataType) => Datas.AddRange(datas);

    #endregion Cosntructors

    #region Events

    /// <summary>
    /// This happens after name changed;
    /// </summary>
    public event EventHandler<EventArgs> NameChanged;
    protected virtual void OnNameChanged(object sender,EventArgs e) {
      //Invoke.
      NameChanged?.Invoke(sender,e);
    }

    #endregion Events

    #region Properties

    /// <summary>
    /// Name.
    /// </summary>
    public virtual string Name {
      get => name;
      set {
        value=value.Trim();
        if(string.IsNullOrWhiteSpace(value))
          throw new MochaException("Name is cannot null or whitespace!");

        Engine_NAMES.CheckThrow(value);

        if(value==name)
          return;

        name=value;
        OnNameChanged(this,new EventArgs());
      }
    }

    /// <summary>
    /// Description.
    /// </summary>
    public virtual string Description { get; set; }

    /// <summary>
    /// Datas of column.
    /// </summary>
    public virtual MochaColumnDataCollection Datas { get; }

    /// <summary>
    /// Data type of datas.
    /// </summary>
    public virtual MochaDataType DataType {
      get => dataType;
      set {
        if(value == dataType)
          return;

        dataType = value;
        Datas.DataType=value;
      }
    }

    /// <summary>
    /// Tag.
    /// </summary>
    public virtual object Tag { get; set; }

    /// <summary>
    /// Text of MHQL AS keyword.
    /// </summary>
    public virtual string MHQLAsText { get; set; }

    #endregion Properties
  }
}
