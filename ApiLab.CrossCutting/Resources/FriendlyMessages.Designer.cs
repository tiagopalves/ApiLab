﻿//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApiLab.CrossCutting.Resources {
    using System;
    
    
    /// <summary>
    ///   Uma classe de recurso de tipo de alta segurança, para pesquisar cadeias de caracteres localizadas etc.
    /// </summary>
    // Essa classe foi gerada automaticamente pela classe StronglyTypedResourceBuilder
    // através de uma ferramenta como ResGen ou Visual Studio.
    // Para adicionar ou remover um associado, edite o arquivo .ResX e execute ResGen novamente
    // com a opção /str, ou recrie o projeto do VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class FriendlyMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal FriendlyMessages() {
        }
        
        /// <summary>
        ///   Retorna a instância de ResourceManager armazenada em cache usada por essa classe.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ApiLab.CrossCutting.Resources.FriendlyMessages", typeof(FriendlyMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Substitui a propriedade CurrentUICulture do thread atual para todas as
        ///   pesquisas de recursos que usam essa classe de recurso de tipo de alta segurança.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a ApiLab - Laboratório.
        /// </summary>
        public static string ApiDescription {
            get {
                return ResourceManager.GetString("ApiDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Encerrando a aplicação....
        /// </summary>
        public static string AppEnding {
            get {
                return ResourceManager.GetString("AppEnding", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Inicializando a aplicação....
        /// </summary>
        public static string AppStarting {
            get {
                return ResourceManager.GetString("AppStarting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Falha na inicialização da aplicação - Erro: .
        /// </summary>
        public static string AppStartingError {
            get {
                return ResourceManager.GetString("AppStartingError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Ocorreu uma falha na sua solicitação. Por favor, tente novamente..
        /// </summary>
        public static string GeneralFail {
            get {
                return ResourceManager.GetString("GeneralFail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Ocorreu uma falha na validação do payload de entrada..
        /// </summary>
        public static string GeneralFailValidation {
            get {
                return ResourceManager.GetString("GeneralFailValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Bad Request.
        /// </summary>
        public static string ProblemDetailsBadRequest {
            get {
                return ResourceManager.GetString("ProblemDetailsBadRequest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Erro na execução do serviço..
        /// </summary>
        public static string ServiceExecutionError {
            get {
                return ResourceManager.GetString("ServiceExecutionError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Consulta uma cadeia de caracteres localizada semelhante a Campo não encontrado..
        /// </summary>
        public static string ValidatorNotFound {
            get {
                return ResourceManager.GetString("ValidatorNotFound", resourceCulture);
            }
        }
    }
}
