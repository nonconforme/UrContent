using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Designer {
    public class ErrorHandler {
        static ErrorHandler inst_;
        public static ErrorHandler inst() { return inst_; }

        public ErrorHandler() {
            inst_ = this;
        }

        public void Error(Exception ex) {
        }

        public void Error(string msg) {
        }
    }
}
