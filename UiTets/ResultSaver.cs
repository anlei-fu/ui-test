using System;
using System.Collections.Generic;

namespace UiTest
{
    public class ResultSaver
    {
        private List<Result> _results = new List<Result>();
        public  void Add(string snapshot,string description)
        {
            _results.Add(new Result(snapshot,description));
        }

        public string Generate()
        {
            return null;
        }

        internal void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
