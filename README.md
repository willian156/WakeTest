# WakeTest

## 📖 Sobre o Projeto

Esta API foi desenvolvida utilizando .NET 8 e segue o padrão de arquitetura Domain-Driven Design (DDD). O banco de dados é o Microsoft SQL Server e é gerenciado com Entity Framework utilizando a abordagem Code-First. Além disso, a aplicação conta com testes unitários e integração contínua via GitHub Actions.

## 🏗️ Tecnologias Utilizadas

- .NET 8  
- Entity Framework Core (Code-First)  
- SQL Server  
- Arquitetura DDD (Domain-Driven Design)  
- Testes Unitários (xUnit)  
- GitHub Actions (CI/CD)  

## 📂 Estrutura do Projeto (DDD)

A organização do código segue a arquitetura DDD, separando as responsabilidades em diferentes camadas:

```sh
📂 WakeTest
 ├── 📂 src
 │   ├── 📂 WakeTest.API          # Camada de Apresentação (Controllers)
 │   ├── 📂 WakeTest.Application  # Casos de uso e serviços
 │   ├── 📂 WakeTest.Domain       # Entidades e interfaces de domínio
 │   ├── 📂 WakeTest.Infrastructure # Persistência de dados e repositórios
 ├── 📂 tests
 │   ├── 📂 WakeTest.UnitTests    # Testes unitários
```

## 🛠️ Configuração do Banco de Dados

A API utiliza Entity Framework Core com o modelo Code-First, ou seja, as tabelas são geradas a partir das entidades do domínio. Para aplicar as migrações e criar o banco de dados, execute os seguintes comandos:

```sh
# Adicionar uma nova migração
dotnet ef migrations add <Nome-da-migration> --project ./WakeTest.Infrastructure/WakeTest.Infrastructure.csproj --startup-project ./WakeTest.API/WakeTest.API.csproj 

# Aplicar a migração ao banco de dados
dotnet ef database update
```

## ✅ Testes Automatizados

Os testes unitários são executados automaticamente via GitHub Actions sempre que houver um push ou pull request no repositório. O pipeline verifica a integridade do código e executa os testes unitários para garantir a estabilidade da aplicação.

Para rodar os testes manualmente:

```sh
dotnet test
```

## 🚀 CI/CD com GitHub Actions

A API possui um workflow configurado no GitHub Actions para rodar os testes automaticamente a cada push ou pull request.
