# 📚 ApiLab

Projeto desenvolvido em **.NET 9**, estruturado seguindo a **Onion Architecture** para promover uma separação clara de responsabilidades, escalabilidade e fácil manutenção.

O projeto é uma API simples que realiza um CRUD de Clientes com Redis, autenticação segura via JWT e API Key, geração de logs estruturados, monitoramento de saúde da API e tratamento centralizado de exceções, com o objetivo de testar as principais novidades do .Net 9 e reforçar conceito de boas práticas de desenvolvimento.

## 🏗️ Estrutura do Projeto

```
ApiLab/
├── 01 - API
│   └── ApiLab.Api
│       ├── Common
│       │   ├── ExceptionHandlers
│       │   └── Filters
│       ├── Controllers
│       ├── Logs
│       ├── appsettings.json
│       └── Program.cs
│
├── 02 - Application
│   └── ApiLab.Application
│       ├── AppServices
│       ├── Commands
│       ├── Extensions
│       └── Validators
│
├── 03 - CrossCutting
│   └── ApiLab.CrossCutting
│       ├── Configurations
│       ├── Issuer
│       ├── LogManager
│       └── Resources
│
├── 04 - Domain
│   └── ApiLab.Domain
│       ├── Entities
│       └── Dependências
│
├── 05 - Infra
│   └── ApiLab.Infra
│       ├── Repository
│       └── Extensions
│
└── 06 - Tests
    └── ApiLab.UnitTests
        ├── Api
        ├── Application
        ├── CrossCutting
        └── Infra
```

## ⚙️ Tecnologias e Frameworks

- **.NET 9** – Plataforma principal
- **Redis** – Utilizado para realização de um CRUD simples
- **Serilog** – Log estruturado (Saída no Console, arquivos `.txt` e Seq)
- **JwtBearer Authentication** – Autenticação via JWT com gerador de tokens
- **HealthChecks e HealthChecksUI** – Monitoramento de saúde da API
- **TypedResults** – Respostas tipadas para endpoints
- **ExceptionHandler** – Manipulação global de exceções
- **ApiKeyValidationFilter** – Filtro personalizado para validação de API Keys simplificado
- **xUnit** – Testes unitários
- **Scalar** – Documentação da API e testes com Scalar
- **Insomnia** – Collections configuradas para testes de endpoints
- **AppSettings** – Configurações centralizadas via appsettings.json

## 📝 Funcionalidades

- Autenticação segura usando JWT e API Key simplificada
- Geração de tokens JWT simplificados com expiração
- Realização de um CRUD de Clientes com Redis
- Geração de logs estruturados em múltiplos destinos
- Monitoramento de saúde da API
- Validação e tratamento centralizado de erros

## 🧪 Testes

- Estruturados com **xUnit**
- Cobertura para camadas de Application, API e Infra
- Testes para filtros e manipuladores de exceção

## 🚀 Como Executar

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/tiagopalves/ApiLab.git
   ```

2. **Configure o ambiente:**
   - Ajuste os arquivos `appsettings.json` com suas credenciais e configurações.

3. **Execute a aplicação:**
   ```bash
   dotnet run --project src/01 - API/ApiLab.Api
   ```

4. **Acesse a API:**
   - `https://localhost:7206/scalar/v1`

## 📊 Monitoramento

- Acesse o HealthChecksUI em `https://localhost:7206/healthchecks-ui`
- Visualize logs estruturados no **Seq** local e arquivos `.txt`

---

💡 **Contribuições são bem-vindas!**

📫 Em caso de dúvidas, entre em contato em [tiagopintoalves@hotmail.com](mailto:tiagopintoalves@hotmail.com).

---

✨ Desenvolvido utilizando .NET 9 e boas práticas de arquitetura e desenvolvimento.

