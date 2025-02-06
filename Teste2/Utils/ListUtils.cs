using Teste2.Model;

namespace Teste2.Utils
{
    public static class ListUtils<T>
    {
        public static bool ValidateList(IList<T>? datas)
        {
            if (datas is not null)
            {
                if (datas.Any())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
