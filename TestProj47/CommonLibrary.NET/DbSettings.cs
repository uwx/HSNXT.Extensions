/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace HSNXT.ComLib.Data
{
    /// <summary>
    /// This class contains database settings.
    /// </summary>
    public class DbSettings
    {
        /// <summary>
        /// Enable transactions flag.
        /// </summary>
        public bool EnableTransactions = false;


        /// <summary>
        /// String to use when prefixing parameters.
        /// </summary>
        public string ParamPrefix = "@";
    }
}
