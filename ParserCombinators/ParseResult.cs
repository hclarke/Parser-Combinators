using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserCombinators {
    public struct ParseResult<TSource, TResult> {
        public readonly TResult value;
        public readonly ParseContext<TSource> context;

        public ParseResult(TResult value, ParseContext<TSource> context) {
            this.value = value;
            this.context = context;
        }

        public ParseResult<TSource, TResult2> Select<TResult2>(Func<TResult, TResult2> selector) {
            return new ParseResult<TSource, TResult2>(selector(value), context);
        }
    }
}
