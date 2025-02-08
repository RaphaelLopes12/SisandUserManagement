# 🚀 Cadastro de Usuários - Sisand

Este projeto é um **Sistema de Gerenciamento de Usuários**, composto por um **Backend em .NET Core** e um **Frontend Angular SPA**. A aplicação permite autenticação de usuários e gerenciamento (CRUD) completo de usuários autenticados.

---

## 📌 Requisitos do Projeto

- Tela de autenticação (usuário e senha)
- Tela de usuários (CRUD completo: listar, criar, alterar e excluir)
- Apenas usuários autenticados podem acessar o CRUD
- Utilização de banco de dados relacional
- Manipulação de dados com **Entity Framework Core**
- Interface construída com **Bootstrap**
- 🚫 **Proibido o uso de scaffold**

---

## 🛠️ Tecnologias Utilizadas

### 🔹 **Backend (.NET 6 - API REST)**
- **.NET Core 6**
- **Entity Framework Core**
- **JWT (JSON Web Token)**
- **Microsoft SQL Server**
- **AutoMapper**
- **xUnit & Moq** (para testes unitários)
- **Docker & Docker Compose**

### 🔹 **Frontend (Angular SPA)**
- **Angular 15+**
- **Bootstrap**
- **RxJS**
- **Angular Router**

---

## ⚡ **Como Rodar o Projeto Localmente**

### **🔧 1. Configurar o Banco de Dados**

Certifique-se de que você tem **SQL Server** instalado ou rode um container com:

```sh
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword123" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
```

Crie o banco de dados manualmente ou deixe a aplicação criar automaticamente ao rodar as migrações.

### **🖥️ 2. Rodando o Backend**

1. Navegue até a pasta do backend:

```sh
cd backend
```

2. Instale as dependências:

```sh
dotnet restore
```

3. Configure a string de conexão no `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost,1433;Database=SisandDB;User Id=sa;Password=YourPassword123;TrustServerCertificate=True;"
}
```

4. Gerar as migrações e aplicar no banco:

```sh
dotnet ef migrations add InitialCreate
```

```sh
dotnet ef database update
```

5. Rode a API:

```sh
dotnet run
```

A API ficará disponível em `http://localhost:5000`.

### **🌐 3. Rodando o Frontend**

1. Navegue até a pasta do frontend:

```sh
cd frontend
```

2. Instale as dependências:

```sh
npm install
```

3. Inicie o servidor de desenvolvimento:

```sh
ng serve
```

O frontend ficará acessível em `http://localhost:4200`.

---

## 🐳 **Rodando com Docker (Opcional)**

Para rodar toda a aplicação com **Docker Compose**, use:

```sh
docker-compose up --build
```

Isso criará os containers do backend, frontend e banco de dados automaticamente.

---

## 🛠 **Endpoints da API**

### 🔑 **Autenticação**
- `POST /api/auth/login` - Login e geração do token JWT.
- `POST /api/auth/register` - Cadastro de um novo usuário.

### 👥 **Usuários**
- `GET /api/users/me` - Retorna os dados do usuário autenticado.
- `GET /api/users` - Lista todos os usuários.
- `PUT /api/users/{id}` - Atualiza um usuário.
- `DELETE /api/users/{id}` - Deleta um usuário.

---

## 📄 **Estrutura do Repositório**

```sh
/sisand-cadastro-usuarios
│── backend/          # API .NET Core
│── frontend/         # Aplicação Angular
│── docker-compose.yml # Configuração do Docker
│── README.md         # Documentação
```

---

## 📢 **Contato**

Se tiver alguma dúvida ou sugestão, sinta-se à vontade para abrir uma **issue** no repositório!

🔗 **LinkedIn:** [Raphael Lopes](https://www.linkedin.com/in/raphaellopesh/)

---

### 💡 **Considerações Finais**

Este projeto foi desenvolvido seguindo as melhores práticas, com foco em **Clean Code** e **boas práticas de desenvolvimento**.

