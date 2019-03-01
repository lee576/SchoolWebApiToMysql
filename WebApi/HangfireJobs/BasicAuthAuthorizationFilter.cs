using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace SchoolWebApi.HangfireJobs
{
    /// <summary>
    /// Hangfire看板登录验证
    /// </summary>
    public class BasicAuthAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly BasicAuthAuthorizationFilterOptions _options;
        private readonly IHostingEnvironment _env;
        private BasicAuthAuthorizationFilterOptions basicAuthAuthorizationFilterOptions;

        /// <summary>
        /// 
        /// </summary>
        public BasicAuthAuthorizationFilter()
            : this(new BasicAuthAuthorizationFilterOptions())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        /// <param name="options"></param>
        public BasicAuthAuthorizationFilter(IHostingEnvironment env, BasicAuthAuthorizationFilterOptions options)
        {
            _options = options;
            _env = env;
        }

        /// <inheritdoc />
        public BasicAuthAuthorizationFilter(BasicAuthAuthorizationFilterOptions basicAuthAuthorizationFilterOptions) =>
            this.basicAuthAuthorizationFilterOptions = basicAuthAuthorizationFilterOptions;

        private bool Challenge(HttpContext context)
        {
            context.Response.StatusCode = 401;
            context.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_context"></param>
        /// <returns></returns>
        public bool Authorize(DashboardContext _context)
        {
            if (_env.IsDevelopment())
                return true;

            var context = _context.GetHttpContext();
            if ((_options.SslRedirect == true) && (context.Request.Scheme != "https"))
            {
                string redirectUri = new UriBuilder("https", context.Request.Host.ToString(), 443, context.Request.Path).ToString();

                context.Response.StatusCode = 301;
                context.Response.Redirect(redirectUri);
                return false;
            }

            if ((_options.RequireSsl == true) && (context.Request.IsHttps == false))
            {
                return false;
            }

            string header = context.Request.Headers["Authorization"];
            if (String.IsNullOrWhiteSpace(header) == false)
            {
                AuthenticationHeaderValue authValues = AuthenticationHeaderValue.Parse(header);
                if ("Basic".Equals(authValues.Scheme, StringComparison.OrdinalIgnoreCase))
                {
                    string parameter = Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter));
                    var parts = parameter.Split(':');
                    if (parts.Length > 1)
                    {
                        string login = parts[0];
                        string password = parts[1];

                        if ((String.IsNullOrWhiteSpace(login) == false) && (String.IsNullOrWhiteSpace(password) == false))
                        {
                            return _options.Users.Any(user => user.Validate(login, password, _options.LoginCaseSensitive))
                                   || Challenge(context);
                        }
                    }
                }
            }
            return Challenge(context);
        }
    }
}
