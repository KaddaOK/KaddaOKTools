using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaddaOK.Library.Tests
{
    public class RzlrcContentsGeneratorTests
    {
        public class BuildInnerTextFromWords
        {
            [Fact]
            public void ShouldTranslateCorrectly()
            {
                var list = new List<LyricWord>
                {
                    new()
                    {
                        StartSecond = 85.65,
                        EndSecond = 86.1,
                        Text = "HE "
                    },
                    new()
                    {
                        StartSecond = 86.1,
                        EndSecond = 86.31,
                        Text = "NE"
                    },
                    new()
                    {
                        StartSecond = 86.31,
                        EndSecond = 86.79,
                        Text = "VER "
                    },
                    new()
                    {
                        StartSecond = 86.79,
                        EndSecond = 87.79,
                        Text = "KNEW "
                    },
                    new()
                    {
                        StartSecond = 89.02,
                        EndSecond = 89.36,
                        Text = "WHAT "
                    },
                    new()
                    {
                        StartSecond = 89.63,
                        EndSecond = 89.91,
                        Text = "HIT "
                    },
                    new()
                    {
                        StartSecond = 89.91,
                        EndSecond = 90.61,
                        Text = "HIM"
                    }
                };
                var result = RzlrcContentsGenerator.BuildInnerTextFromWords(list);
                Assert.Equal("HE <86.1>NE<86.31>VER <86.79>KNEW <87.79+1.23>WHAT <89.36+0.27>HIT <89.91>HIM",
                    result);
            }
        }
    }
}
