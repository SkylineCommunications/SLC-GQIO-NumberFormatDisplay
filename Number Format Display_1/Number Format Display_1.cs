namespace Number_Format_Display_1
{
    using System;
    using Skyline.DataMiner.Analytics.GenericInterface;

    /// <summary>
    /// Represents a DataMiner Automation script.
    /// </summary>
    [GQIMetaData(Name = "Number Format Display")]
    public class NumberFormatDisplay : IGQIColumnOperator, IGQIOperator, IGQIRowOperator, IGQIInputArguments
    {
        private readonly GQIColumnDropdownArgument _COL_ARG = new GQIColumnDropdownArgument("Column")
        {
            IsRequired = true,
        };

        private GQIStringArgument _nameArg = new GQIStringArgument("Column name")
        {
            IsRequired = true,
        };

        private GQIColumn _oldColumn;
        private GQIStringColumn _newColumn;

        public GQIArgument[] GetInputArguments()
        {
            return new GQIArgument[]
            {
                this._COL_ARG,
                this._nameArg,
            };
        }

        public OnArgumentsProcessedOutputArgs OnArgumentsProcessed(OnArgumentsProcessedInputArgs args)
        {
            this._oldColumn = args.GetArgumentValue<GQIColumn>(this._COL_ARG);
            this._newColumn = new GQIStringColumn(args.GetArgumentValue(this._nameArg));
            return new OnArgumentsProcessedOutputArgs();
        }

        public void HandleColumns(GQIEditableHeader header)
        {
            header.AddColumns(new GQIColumn[]
            {
                this._newColumn,
            });
        }

        public void HandleRow(GQIEditableRow row)
        {
            double value = row.GetValue<double>(this._oldColumn);
            if (value < 10000)
            {
                if (value < 1000)
                {
                    row.SetValue<string>(this._newColumn, Convert.ToString(value), null);
                }
                else
                {
                    row.SetValue<string>(this._newColumn, Convert.ToString(value / 1000).Replace(".", ","), null);
                }
            }
            else if (value >= 10000 && value < 1000000)
            {
                row.SetValue<string>(this._newColumn, Convert.ToString(String.Format("{0}K", Math.Round(value / 1000, 1))).Replace(".", ","), null);
            }
            else
            {
                row.SetValue<string>(this._newColumn, Convert.ToString(String.Format("{0}M", Math.Round(value / 1000000, 1))).Replace(".", ","), null);
            }
        }
    }
}