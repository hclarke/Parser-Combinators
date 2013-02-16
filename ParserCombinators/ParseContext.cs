using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParserCombinators {
    public class ParseContext<TSource> {
        private readonly TSource value;

        public TSource Value {
            get { return this.value; }
        } 

        readonly Func<ParseContext<TSource>> next;
        public ParseContext<TSource> Next {
            get { return next(); }
        }
        public ParseContext(TSource value, Func<ParseContext<TSource>> next) {
            this.value = value;
            this.next = next;
        }

        public static bool operator true(ParseContext<TSource> context) {
            return context != null;
        }
        public static bool operator false(ParseContext<TSource> context) {
            return context == null;
        }

        public static implicit operator TSource(ParseContext<TSource> context) {
            return context.value;
        }
    }

    public static class ParseContexts {
        public static ParseContext<T> Create<T>(IReadOnlyList<T> source, int start = 0) {
            if (source == null) throw new ArgumentNullException("source");
            if (start >= source.Count) return null;
            return new ParseContext<T>(
                source[start],
                () => Create(source, start + 1));
        }
    }
}
