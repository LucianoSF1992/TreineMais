🇺🇸 **English version below** 

# 💪 Treine+ (TreineMais)

![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![ASP.NET](https://img.shields.io/badge/ASP.NET-Core-blue)
![SQL Server](https://img.shields.io/badge/Database-SQLServer-red)
![License](https://img.shields.io/badge/license-MIT-green)

A web-based platform designed for gyms and personal trainers to manage customized workout plans for their students.

This project was developed as an **MVP (Minimum Viable Product)** using **ASP.NET Core MVC** and **SQL Server**, with authentication powered by **ASP.NET Identity**.

🌐 Live Demo:
https://treinemais.lucianoferreiradev.com

# 🏗 Architecture

The project follows a **layered architecture**, separating responsibilities into:

* **Controllers** → Handle HTTP requests
* **Services (future implementation)** → Business logic
* **Models / Entities** → Domain entities representation
* **ViewModels** → Communication between Controllers and Views
* **Views (Razor)** → User interface

Data access is implemented using **Entity Framework Core** with the **Code First** approach and **Migrations**.

---

# 🚀 Technologies Used

## Backend

* .NET 8
* ASP.NET Core MVC
* Entity Framework Core
* ASP.NET Identity

## Database

* SQL Server

## Frontend

* Razor Views
* Bootstrap 5

## Infrastructure

* Linux VPS
* Nginx
* Docker

---

# 🎯 Project Goal

Enable **instructors** to create and manage personalized workout plans for their students, while allowing **students** to track their workouts and mark exercises as completed.

The project was built with a focus on:

* Clean architecture
* Secure authentication
* Clear user workflows
* Scalability for future enhancements

---

# 👥 User Roles

## 👨‍🏫 Instructor

Can:

* Register students
* Create workout plans
* Assign workouts by day of the week
* Manage students

## 👨‍🎓 Student

Can:

* View assigned workouts
* Track progress
* Mark workouts as completed

---

# 🔐 Demo Accounts

To make system evaluation easier, three demo accounts are available.

## 👑 Admin

Responsible for system administration.

**Email:** [admin@treinemais.com](mailto:admin@treinemais.com)
**Password:** Admin@123

## 👨‍🏫 Instructor

Can register students and create personalized workout plans.

**Email:** [instrutor@treinemais.com](mailto:instrutor@treinemais.com)
**Password:** Instrutor@123

## 👨‍🎓 Student

Can view workouts and mark exercises as completed.

**Email:** [aluno@treinemais.com](mailto:aluno@treinemais.com)
**Password:** Aluno@123

---

# 🧱 MVP Features

* Authentication with ASP.NET Identity
* Role-based authorization (Admin / Instructor / Student)
* Student registration and management
* Personalized workout creation
* Exercise management
* Workout visualization for students
* Exercise completion tracking
* Responsive user interface

---

# 🗂 Project Structure

```text
TreineMais
│
├── Controllers
│ ├── AdminController.cs
│ ├── DashboardController.cs
│ ├── ExerciciosController.cs
│ ├── HomeController.cs
│ ├── RedirectController.cs
│ └── TreinosController.cs
│
├── Data
│ ├── AppDbContext.cs
│ ├── IdentitySeed.cs
│ └── Migrations
│
├── Models
│ ├── ApplicationUser.cs
│ ├── ErrorViewModel.cs
│ ├── Exercicio.cs
│ ├── Treino.cs
│ └── TreinoExercicio.cs
│
├── ViewModels
│
├── Views
│
├── Areas
│ └── Identity
│ └── Pages
│ └── Account
│
├── Properties
│
└── Program.cs
```

---

# ⚙️ Running the Project Locally

### 1️⃣ Clone the repository

```bash
git clone https://github.com/LucianoSF1992/TreineMais.git
```

### 2️⃣ Navigate to the project folder

```bash
cd TreineMais
```

### 3️⃣ Configure the connection string

Open:

```text
appsettings.json
```

Configure it to point to your local SQL Server instance.

---

### 4️⃣ Apply database migrations

```bash
dotnet ef database update
```

---

### 5️⃣ Run the application

```bash
dotnet run
```

---

# 🗄 Database

Database engine:

**SQL Server**

ORM:

**Entity Framework Core**

Main entities:

* ApplicationUser
* Treino (Workout)
* Exercicio (Exercise)
* TreinoExercicio (WorkoutExercise)

---

# 🚀 Deployment

The application is hosted on a manually configured **Linux VPS**.

Infrastructure:

* **Ubuntu Server**
* **Nginx** (Reverse Proxy)
* **.NET Runtime**
* **SQL Server running in Docker**
* **SSL Certificate (Let's Encrypt)**

Deployment architecture:

```text
Internet
    ↓
Nginx (Reverse Proxy)
    ↓
ASP.NET Core (Kestrel)
    ↓
SQL Server
```

---

# 📸 Screenshots

### Login

![Login](docs/Tela-de-login.png)

### Admin Dashboard

![Dashboard Admin](docs/dashboard-admin.png)

### Student Dashboard

![Dashboard Student](docs/dashboard-aluno.png)

### Create Workout

![Create Workout](docs/Tela-criar-treino.png)

### Create Student

![Create Student](docs/Tela-criar-aluno.png)

---

# 🔮 Future Improvements

* Workout history
* Performance analytics and charts
* Exercise video uploads
* REST API with JWT authentication
* Mobile application (MAUI or React Native)
* Full administrative panel

---

# 👨‍💻 Author

**Luciano Ferreira**
Full Stack .NET Developer

🌐 Portfolio
https://lucianoferreiradev.com

💼 LinkedIn
https://www.linkedin.com/in/lucianoferreira92/

💻 GitHub
https://github.com/LucianoSF1992



🇧🇷 **Versão em português abaixo**

# 💪 Treine+ (TreineMais)

![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![ASP.NET](https://img.shields.io/badge/ASP.NET-Core-blue)
![SQL Server](https://img.shields.io/badge/Database-SQLServer-red)
![License](https://img.shields.io/badge/license-MIT-green)

Sistema web para academias e personal trainers gerenciarem treinos personalizados de alunos.

Projeto desenvolvido como **MVP (Minimum Viable Product)** utilizando **ASP.NET Core MVC** e **SQL Server**, com autenticação baseada em **Identity**.

🌐 Sistema online:  
https://treinemais.lucianoferreiradev.com

# 🏗 Arquitetura

O projeto segue uma arquitetura baseada em **camadas**, separando responsabilidades entre:

- **Controllers** → controle das requisições HTTP
- **Services (futuro)** → regras de negócio
- **Models / Entities** → representação das entidades do sistema
- **ViewModels** → comunicação entre Controller e View
- **Views (Razor)** → interface do usuário

O acesso a dados é realizado através do **Entity Framework Core**, utilizando o padrão **Code First com Migrations**.

---

# 🚀 Tecnologias Utilizadas

Backend
- .NET 8
- ASP.NET Core MVC
- Entity Framework Core
- ASP.NET Identity

Banco de Dados
- SQL Server

Frontend
- Razor Views
- Bootstrap 5

Infraestrutura
- Linux VPS
- Nginx
- Docker

---

# 🎯 Objetivo do Projeto

Permitir que **instrutores** criem e gerenciem treinos personalizados para seus alunos, enquanto os **alunos acompanham seus treinos e marcam exercícios como concluídos.**

O projeto foi desenvolvido com foco em:

- arquitetura simples
- autenticação segura
- fluxo claro de usuário
- base preparada para evolução futura

---

# 👥 Perfis de Usuário

## 👨‍🏫 Instrutor

Pode:

- cadastrar alunos
- criar treinos
- atribuir treinos por dia da semana
- gerenciar alunos

## 👨‍🎓 Aluno

Pode:

- visualizar seus treinos
- acompanhar progresso
- marcar treinos como concluídos

---

## 🔐 Usuários de Teste

Para facilitar a avaliação do sistema, foram criados três perfis de usuário.

### 👑 Admin
Responsável pela visão administrativa do sistema.

Email: admin@treinemais.com  
Senha: Admin@123


### 👨‍🏫 Instrutor
Pode cadastrar alunos e criar treinos personalizados.

Email: instrutor@treinemais.com  
Senha: Instrutor@123


### 👨‍🎓 Aluno
Pode visualizar seus treinos e marcar exercícios como concluídos.

Email: aluno@treinemais.com  
Senha: Aluno@123
---

# 🧱 Funcionalidades do MVP

- Autenticação com ASP.NET Identity
- Controle de acesso por perfil (Admin / Instrutor / Aluno)
- Cadastro e gerenciamento de alunos
- Criação de treinos personalizados
- Cadastro de exercícios
- Visualização de treinos pelos alunos
- Marcação de exercícios concluídos
- Interface responsiva 

---

# 🗂 Estrutura do Projeto

```text
TreineMais
│
├── Controllers
│ ├── AdminController.cs
│ ├── DashboardController.cs
│ ├── ExerciciosController.cs
│ ├── HomeController.cs
│ ├── RedirectController.cs
│ └── TreinosController.cs
│
├── Data
│ ├── AppDbContext.cs
│ ├── IdentitySeed.cs
│ └── Migrations
│
├── Models
│ ├── ApplicationUser.cs
│ ├── ErrorViewModel.cs
│ ├── Exercicio.cs
│ ├── Treino.cs
│ └── TreinoExercicio.cs
│
├── ViewModels
│
├── Views
│
├── Areas
│ └── Identity
│ └── Pages
│ └── Account
│
├── Properties
│
└── Program.cs
```

---

# ⚙️ Como Rodar o Projeto Localmente

### 1️⃣ Clone o repositório

git clone https://github.com/LucianoSF1992/TreineMais.git

### 2️⃣ Entre na pasta do projeto

cd treinemais

### 3️⃣ Configure a connection string

Arquivo:

appsettings.json

Configure para seu SQL Server local.

---

### 4️⃣ Execute as migrations

dotnet ef database update

---

### 5️⃣ Rode o projeto

dotnet run


---

# 🗄 Banco de Dados

Banco utilizado:

**SQL Server**

Gerenciado por:

**Entity Framework Core**

Principais entidades:

- ApplicationUser
- Treino
- Exercicio
- TreinoExercicio

---

# 🚀 Deploy

O sistema está hospedado em uma **VPS Linux** configurada manualmente.

Infraestrutura utilizada:

- **Ubuntu Server**
- **Nginx** (Reverse Proxy)
- **.NET Runtime**
- **SQL Server em container Docker**
- **Certificado SSL (Let's Encrypt)**

Arquitetura de deploy:

Internet
↓
Nginx (Reverse Proxy)
↓
ASP.NET Core (Kestrel)
↓
SQL Server

---

# 📸 Screenshots

### Login

![Login](docs/Tela-de-login.png)

### Dashboard - Admin

![Dashboard Admin](docs/dashboard-admin.png)

### Dashboard - Aluno

![Dashboard Aluno](docs/dashboard-aluno.png)

### Criar Treino

![Criar Treino](docs/Tela-criar-treino.png)

### Criar Aluno

![Criar Aluno](docs/Tela-criar-aluno.png)


# 🔮 Próximas Evoluções

- Histórico de treinos
- Gráficos de desempenho
- Upload de vídeos de exercícios
- API REST com JWT
- Aplicativo mobile (MAUI ou React Native)
- Área administrativa completa

---

# 👨‍💻 Autor

Luciano Ferreira  
Desenvolvedor Full Stack .NET

🌐 Portfólio  
https://lucianoferreiradev.com

💼 LinkedIn  
https://www.linkedin.com/in/lucianoferreira92/

💻 GitHub  
https://github.com/LucianoSF1992