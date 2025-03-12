# Instruções para rodar

A aplicação está configurada com docker e docker compose, então basta clonar o repositorio e rodar um **docker compose up --build**, isso irá gerar:

- Um container com Postgres rodando na porta **5432**
- Um container para a API rodando na porta **12271**, sendo possivel acessar o swager utilizando: *http://localhost:12271/swagger/index.html*
- Um container com a aplicação em angular **v19**, sendo possivel acessar pela seguinte URL: *http://localhost:8080/login*


# Instruções para acessar

A aplicação tem um sistema de login simples, utilizando JWT para tokenização e os seguintes usuários com suas respectivas permissões:

- Usuário Admin, com a permissão Director
 > documentNumber: 12345678901 / password: Admin@123
- Usuário Manager, com a permissão Leader
> documentNumber: 10987654321 / password: Manager@123
- Usuário Employee, com a permissão Employee
> documentNumber: 12345678910 / password: Employee@123


## Arquitetura Backend

Inicialmente iria criar uma arquitetura hexagonal, mas pelo tempo ser pequeno e por ser uma aplicação de pouca complexidade, optei por criar a aplicação usando uma arquitetura simples em camadas junto com DDD. Temos o EF acessando o banco de dados Postgre, migrations para inicialização da base, Serilog para lidar com os logs (está configurado para salvar no console), FluentValidations para validação das DTOs, JWT para tokenização da autenticação, xUnit e Moq para os testes unitários, tudo isso rodando em cima do .NET 8.

## Arquitetura Frontend

O frontend está separado por paginas (pages), as partes vitais da aplicação estão dentro de core e os serviços compartilhados em shared e foi utilizado o MaterialUI para design da interface, tudo rodando em cima do Angular v19.
