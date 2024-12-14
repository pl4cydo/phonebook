# PhoneBook

**PhoneBook** é uma aplicação de agenda telefônica que permite gerenciar contatos de forma simples e eficaz. Esta aplicação é composta por três projetos principais:

- **PhoneBookApi**: Backend desenvolvido com .NET 8 e ASP.NET Core.
- **PhoneBookPage**: Frontend desenvolvido com Vue 3 e PrimeVue.
- **PhoneBookTest**: Projeto de testes para a API.
- **Banco de Dados**: MySQL para armazenamento de dados.

## Funcionalidades

- Adicionar, editar e excluir contatos.
- Listar contatos armazenados no banco de dados.
- Interface de usuário moderna e responsiva.

## Tecnologias Utilizadas

- **Backend**: .NET 8, ASP.NET Core
- **Frontend**: Vue 3, PrimeVue
- **Banco de Dados**: MySQL

## Iniciando o Projeto pelo Docker Compose

Para iniciar todos os projetos de uma vez utilizando o Docker, siga os passos abaixo:

1. Execute o comando a seguir para construir e iniciar os containers:
    ```bash
    sudo docker-compose up --build
    ```
Acesse [http://localhost:5173/
](http://localhost:5173/)

OBS: Caso seu mysql esteja up é preciso parar o processo. Caso alguma dessa portas estejam em uso, sugiro fazer o mesmo: 5173, 5011 e 3306.

## Como Iniciar Cada Projeto Separadamente

### Frontend

Para iniciar o frontend, siga os passos abaixo:

1. Navegue até o diretório do projeto frontend:
   ```bash
    cd PhoneBookPage
    npm install
    npm run serve
    ```
Acesse [http://localhost:5173/
](http://localhost:5173/
)

### Backend

1. Navegue até o diretório do projeto backend:
    ```bash 
    cd PhoneBookApi
    dotnet restore
    dotnet run
    ```
Acesse [http://localhost:5011/api/Contacts/
](http://localhost:5011/api/Contacts/
)

### Banco MySQL

1. No terminal:
    ```bash
    mysql -u placydo -p placydo
    ```