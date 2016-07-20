namespace Lemon.Transform
{
    public enum DataRowCompareStatus
    {
        New = 0,
        Changed = 1,
        NoChange = 2
    }

    public abstract class DataRowStatusContext
    {
        protected string[] PrimaryKeys;

        protected string[] ColumnsToCompare;

        public DataRowStatusContext(string[] primaryKeys, string[] columnsToCompare)
        {
            PrimaryKeys = primaryKeys;

            ColumnsToCompare = columnsToCompare;
        }

        protected abstract BsonDataRow GetTargetRow(BsonDataRow row);

        public DataRowCompareStatus Compare(BsonDataRow row)
        {
            var targetRow = GetTargetRow(row);

            if (targetRow == null)
            {
                return DataRowCompareStatus.New;
            }

            foreach (var column in ColumnsToCompare)
            {
                if (!row.GetValue(column).Equals(targetRow.GetValue(column)))
                {
                    return DataRowCompareStatus.Changed;
                }
            }

            return DataRowCompareStatus.NoChange;
        }
    }
}
