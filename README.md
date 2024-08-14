# MarketView

![Angular](https://img.shields.io/badge/Angular-E23237?style=flat-square&logo=angular&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-5a9bd4?style=flat-square&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/C%23-0078d4?style=flat-square&logo=csharp&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-DC382D?style=flat-square&logo=redis&logoColor=white)

MarketView é um site para buscar ativos financeiros e visualizar notícias relacionadas. A aplicação permite consultar os ativos mais negociados e as últimas notícias do mercado. Desenvolvido com Angular para o frontend e .NET com C# e Redis para o backend.

## Funcionalidades

- **Buscar Ativos**: Encontre ativos financeiros com base em critérios específicos.
- **Visualizar Notícias**: Acesse as notícias mais recentes relacionadas a ativos.
- **Consultar Ativos Mais Negociados**: Veja quais ativos estão mais em alta no mercado.
- **Últimas Notícias do Mercado**: Mantenha-se atualizado com as últimas notícias do mercado financeiro.

## Tecnologias Utilizadas

- **Frontend**: [Angular](https://angular.io/)
- **Backend**: [.NET](https://dotnet.microsoft.com/) e [C#](https://learn.microsoft.com/en-us/dotnet/csharp/) com [Redis](https://redis.io/)

## Instalação

### Backend (.NET)

1. Clone o repositório:

 ```bash
 git clone https://github.com/eduardokroetz/marketview.git
```

2. Navegue até a pasta do projeto:

```bash
  cd marketview
```

2. Execute o docker-compose.yml (é necessário ter o docker instalado e iniciado)

```bash
  docker-compose up 
```

### Frontend (Angular)

1. Navegue até a pasta do frontend:

```bash
  cd marketview/frontend/marketview
```

2. Instale as dependências:

```bash
  npm install
```

3.Execute a aplicação:

```bash
  ng serve
```

O frontend estará disponível em http://localhost:4200.
