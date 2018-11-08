using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace Web.Common.HtmlHelpers
{
    public static class HtmlHelpers
    {
        /// <summary>
        /// Botão para exibição de modal.
        /// </summary>
        /// <param name="ajaxHelper">Extensão.</param>
        /// <param name="modalButtonParameters">Parâmetro do ModalButton.</param>
        /// <returns></returns>
        public static MvcHtmlString ModalButton(this AjaxHelper ajaxHelper, ModalButtonParameters modalButtonParameters)
        {
            AjaxOptions ajaxOptions = new AjaxOptions
            {
                HttpMethod = "get",
                UpdateTargetId = "ModalContent",
                LoadingElementId = "divCarregando",
                OnBegin = modalButtonParameters.OnBegin,
                OnSuccess = "comprasnet.showModal('" + modalButtonParameters.ModalTitle + "'," + modalButtonParameters.Responsive.ToString().ToLower(CultureInfo.InvariantCulture) + ", null, '" + modalButtonParameters.ModalWidth + "', '" + modalButtonParameters.Modalheight + "')",
                InsertionMode = InsertionMode.Replace
            };

            return ajaxHelper.ActionLink(modalButtonParameters.LinkText ?? " ", modalButtonParameters.ActionName, modalButtonParameters.ControllerName, modalButtonParameters.RouteValues, ajaxOptions, modalButtonParameters.HtmlAttributes);
        }

        /// <summary>
        /// Gera um elemento a (link) com a formatação padrão do botão de imprimir e abre em uma nova aba (target _blank)
        /// </summary>
        /// <param name="printTitle">Título da página de impressão</param>
        /// <param name="action">Action</param>
        /// <param name="routeValues">Parãmetros a serem passados para o controller</param>
        /// <returns></returns>
        public static MvcHtmlString PrintButton(this HtmlHelper htmlHelper, string printTitle, string action, object routeValues)
        {
            TagBuilder tagBuilder;
            UrlHelper urlHelper;
            var routeValuesLocal = routeValues as IDictionary<string, object> ?? new RouteValueDictionary(routeValues);
            urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            tagBuilder = new TagBuilder("a");

            tagBuilder.InnerHtml = "Imprimir";
            tagBuilder.AddCssClass("btn btn-default yellow medium");
            tagBuilder.Attributes.Add("target", "_blank");
            routeValuesLocal.Add("PrintTitle", printTitle);

            tagBuilder.Attributes["href"] = urlHelper.Action(action, new RouteValueDictionary(routeValuesLocal));
            return MvcHtmlString.Create(tagBuilder.ToString());
        }

        public static IHtmlString RenderScripts(this HtmlHelper htmlHelper)
        {
            var scripts = htmlHelper.ViewContext.HttpContext.Items["Scripts"] as IList<string>;
            string scriptsAtual = String.Empty;
            if (scripts != null)
            {
                var builder = new StringBuilder();
                foreach (var script in scripts)
                {
                    if (!script.Equals(scriptsAtual))
                    {
                        scriptsAtual = script;
                        builder.AppendLine(script);
                    }

                }
                return new MvcHtmlString(builder.ToString());
            }
            return null;
        }

       
      
      
    }
}
