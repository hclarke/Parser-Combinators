using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserCombinators {
    public static class Parsers {

        public static Parser<T, T> Any<T>() {
            return new Parser<T, T>(context => {
                if (context) {
                    return new ParseResult<T, T>(
                        context.Value,
                        context.Next);
                }
                else {
                    throw new ParseException<T>("Unexpected end of input", context, context);
                }
            });
        }

        public static Parser<T, T> Any<T>(Func<T, bool> f) {
            return Any<T>().Where(f);
        }

        public static Parser<T, IEnumerable<R>> Many<T, R>(this Parser<T, R> parser) {
            return new Parser<T, IEnumerable<R>>(c => {
                var results = new List<R>();
                ParseResult<T,R> r;
                while (parser.TryParse(c, out r)) {
                    results.Add(r);
                    c = r.context;
                }
                return new ParseResult<T, IEnumerable<R>>(results, c);
            });
        }

        public static Parser<T, IEnumerable<R>> Many1<T,R>(this Parser<T, R> parser) {
            return
                from x in parser
                from xs in parser.Many()
                select new[] { x }.Concat(xs);
        }

        public static bool TryParse<T, R>(this Parser<T, R> parser, ParseContext<T> context,  out ParseResult<T, R> result) {
            try {
                result = parser.Parse(context);
                return true;
            } catch (ParseException<T> e) {
                result = default(ParseResult<T, R>);
                return false;
            }
        }
        public static ParseResult<T, R> Parse<T, R>(this Parser<T, R> parser, IReadOnlyList<T> data) {
            var context = ParseContexts.Create(data);
            return parser.Parse(context);
        }
    }
}
