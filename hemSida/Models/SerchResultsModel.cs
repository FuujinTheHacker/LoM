using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace hemSida.Models
{
    public class SerchResultsModel : ModelBass
    {
        const int maxTag = int.MaxValue; // avänds ej en nu
        const int maxWord = int.MaxValue; // avänds ej en nu

        public string searchText;
        public int searchAntal;
        public int searchSida;
        public bool searchPrisSort;

        public int antalsidor;

        HashSet<long> hashSet;

        public List<SerchResult> SerchResultsList = new List<SerchResult>();

        public SerchResultsModel(string searchText, int searchAntal, int searchSida, bool searchPrisSort)
        {
            if (searchAntal != 15 && searchAntal != 24 && searchAntal != 48 && searchAntal != 75 && searchAntal != 105)
                searchAntal = 15;

            this.searchText = searchText;
            this.searchAntal = searchAntal;
            this.searchSida = Math.Max(searchSida, 0);
            this.searchPrisSort = searchPrisSort;

            HashSet<string> tagHash = new HashSet<string>();
            HashSet<string> wordHash = new HashSet<string>();

            Filter(tagHash, wordHash, searchText);

            using (SqlConnection con = getCon())
            {
                using (SqlCommand command = new SqlCommand("", con))
                {
                    if (tagHash.Count == 0 && wordHash.Count == 0)
                        all(con);
                    else
                        search(tagHash, wordHash, con);
                }
            }

            if (searchPrisSort)
                SerchResultsList.Sort(new IComparerSerchResultPris());
            else
                SerchResultsList.Sort(new IComparerSerchResultDisplayName());

            if (SerchResultsList.Count != 0)
                antalsidor = (int)Math.Ceiling((double)SerchResultsList.Count / (double)searchAntal);
            else
                antalsidor = 1;
        }

        private void all(SqlConnection con)
        {
            using (SqlCommand command = new SqlCommand("", con))
            {
                command.CommandText = "SELECT ID, DisplayName, ProductPrice FROM [dbo].[Product];";
                command.Connection.Open();

                using (SqlDataReader sqlr = command.ExecuteReader())
                {
                    while (sqlr.Read())
                        SerchResultsList.Add(new SerchResult(sqlr));
                }
            }
        }

        private void search(HashSet<string> tagHash, HashSet<string> wordHash, SqlConnection con)
        {
            int index;
            hashSet = new HashSet<long>();

            if (wordHash.Count != 0)
            {
                index = 0;
                using (SqlCommand command = new SqlCommand("SELECT ID, DisplayName, ProductPrice FROM [dbo].[Product] WHERE", con))
                {
                    foreach (var item in wordHash)
                    {
                        if (index == 0)
                            command.CommandText += " [DisplayName] LIKE @Name" + index;
                        else
                            command.CommandText += " OR [DisplayName] LIKE @Name" + index;
                        command.Parameters.AddWithValue("Name" + index, "%" + item + "%");
                        index++;
                    }

                    command.Connection.Open();

                    using (SqlDataReader sqlr = command.ExecuteReader())
                    {
                        while (sqlr.Read())
                        {
                            SerchResult r = new SerchResult(sqlr);
                            hashSet.Add(r.id);
                            SerchResultsList.Add(r);
                        }
                    }
                }
            }

            if (tagHash.Count != 0)
            {
                index = 0;
                using (SqlCommand command = new SqlCommand(
                    "SELECT j3.ID, j3.DisplayName, j3.ProductPrice FROM [dbo].[TagTyp] AS j1 " +
                "RIGHT OUTER JOIN [dbo].[Product_has_TagTyp] AS j2 ON j1.[ID] = j2.[TagTyp_ID] " +
                "RIGHT OUTER JOIN [dbo].[Product] AS j3 ON j2.[Product_ID] = j3.[ID] " +
                    "WHERE ", con))
                {
                    foreach (var item in tagHash)
                    {
                        if (index == 0)
                            command.CommandText += " j1.[DisplayName] LIKE @tag" + index;
                        else
                            command.CommandText += " OR j1.[DisplayName] LIKE @tag" + index;
                        command.Parameters.AddWithValue("tag" + index, "%" + item + "%");
                        index++;
                    }

                    if (command.Connection.State != ConnectionState.Open)
                        command.Connection.Open();

                    using (SqlDataReader sqlr = command.ExecuteReader())
                    {
                        while (sqlr.Read())
                        {
                            SerchResult r = new SerchResult(sqlr);
                            if (hashSet.Add(r.id))
                                SerchResultsList.Add(r);
                        }
                    }
                }
                hashSet = null;
            }
        }

        static void Filter(HashSet<string> tagHash, HashSet<string> wordHash, string searchText)
        {
            if (searchText == null || searchText == "")
                return;

            bool isTag = false;
            int index = -0;
            int start = 0;

            while (true)
            {
                while (true)
                {
                    if (index >= searchText.Length)
                        return;

                    if (char.IsLetterOrDigit(searchText[index]))
                    {
                        isTag = false;
                        start = index;
                        break;
                    }

                    if (searchText[index] == '#')
                    {
                        isTag = true;
                        index++;
                        start = index;
                        break;
                    }

                    index++;
                }

                while (true)
                {
                    if (index >= searchText.Length)
                    {
                        if (index - start != 0)
                            if (isTag)
                                tagHash.Add(searchText.Substring(start, index - start));
                            else
                                wordHash.Add(searchText.Substring(start, index - start));
                        return;
                    }

                    if (Char.IsWhiteSpace(searchText[index]))
                    {
                        if (isTag)
                            tagHash.Add(searchText.Substring(start, index - start));
                        else
                            wordHash.Add(searchText.Substring(start, index - start));
                        index++;
                        break;
                    }

                    if (searchText[index] == '#')
                    {
                        if (isTag)
                            tagHash.Add(searchText.Substring(start, index - start));
                        else
                            wordHash.Add(searchText.Substring(start, index - start));
                        isTag = true;
                        index++;
                        start = index;
                    }
                    else
                        index++;
                }
            }
        }

    }

    public class SerchResult
    {
        public long id;
        public string displayName;
        public double productPrice;
        public string link_Key;

        public SerchResult(SqlDataReader sqlr)
        {
            id = sqlr.GetInt64(0);
            displayName = sqlr.GetString(1);
            productPrice = sqlr.GetDouble(2);
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            SerchResult r = (SerchResult)obj;
            return r.id == id;
        }

    }

    public class IComparerSerchResultPris : IComparer<SerchResult>
    {
        public int Compare(SerchResult x, SerchResult y)
        {
            return x.productPrice.CompareTo(y.productPrice);
        }
    }

    public class IComparerSerchResultDisplayName : IComparer<SerchResult>
    {
        public int Compare(SerchResult x, SerchResult y)
        {
            int back = x.displayName.CompareTo(y.displayName);
            return back;
        }
    }
}