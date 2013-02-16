using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserCombinators
{
    public delegate ParseResult<TSource, TResult> ParseFunc<TSource, TResult>(ParseContext<TSource> context);

    public class ParseException<TSource> : Exception {
        public readonly ParseContext<TSource> start,end;
        public ParseException(string message, ParseContext<TSource> start, ParseContext<TSource> end) : base(message) {
            this.start = start;
            this.end = end;
        }
    }
    public sealed class Parser<TSource, TResult> {
        readonly ParseFunc<TSource,TResult> parseFunc;

        public Parser(ParseFunc<TSource, TResult> parseFunc) {
            if (parseFunc == null) throw new ArgumentNullException("parseFunc");
            this.parseFunc = parseFunc;
        }
        public ParseResult<TSource, TResult> Parse(ParseContext<TSource> context) {
            return parseFunc(context);
        }

        public static Parser<TSource, TResult> operator |(Parser<TSource, TResult> first, Parser<TSource, TResult> second) {
            return new Parser<TSource, TResult>(context => {
                try {
                    return first.parseFunc(context);
                }
                catch(ParseException<TSource> error) {
                    //eat parse errors
                }
                return second.parseFunc(context);
            });
        }

        public Parser<TSource, TResult2> Select<TResult2>(Func<TResult, TResult2> selector) {
            return new Parser<TSource, TResult2>(context => parseFunc(context).Select(selector));
        }

        public Parser<TSource, TResult3> SelectMany<TResult2, TResult3>(Func<TResult, Parser<TSource, TResult2>> project, Func<TResult, TResult2, TResult3> select) {
            return new Parser<TSource, TResult3>(context => {
                var r1 = parseFunc(context);
                var r2 = project(r1.value).parseFunc(r1.context);
                return new ParseResult<TSource, TResult3>(select(r1.value, r2.value), r2.context);
            });
        }

        public Parser<TSource, TResult> Where(Func<TResult, bool> pred) {
            return new Parser<TSource, TResult>(context => {
                var r = parseFunc(context);
                if (pred(r.value)) {
                    throw new ParseException<TSource>("Where failed: " + pred.Method.Name, context, r.context);
                }
                return r;
            });
        }
    }
}
