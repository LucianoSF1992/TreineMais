# 💪 Treine+ (TreineMais)

![.NET](https://img.shields.io/badge/.NET-9.0-purple)
![ASP.NET](https://img.shields.io/badge/ASP.NET-Core-blue)
![SQL Server](https://img.shields.io/badge/Database-SQLServer-red)
![License](https://img.shields.io/badge/license-MIT-green)

Sistema web para academias e personal trainers gerenciarem treinos personalizados de alunos.

Projeto desenvolvido como **MVP (Minimum Viable Product)** utilizando **ASP.NET Core MVC** e **SQL Server**, com autenticação baseada em **Identity**.

🌐 Sistema online:  
https://treinemais.lucianoferreiradev.com

---

# 🚀 Tecnologias Utilizadas

- .NET 9
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- ASP.NET Identity
- Bootstrap 5
- Razor Views

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

Para facilitar a avaliação do sistema, foram criados três perfis de usuário:

### 👑 Admin

Responsável pela visão administrativa do sistema.

Email  
admin@treinemais.com

Senha  
Admin123!

---

### 👨‍🏫 Instrutor

Pode cadastrar alunos e criar treinos personalizados.

Email  
instrutor@treinemais.com

Senha  
Instrutor123!

---

### 👨‍🎓 Aluno

Pode visualizar seus treinos e marcar exercícios como concluídos.

Email  
aluno@treinemais.com

Senha  
Aluno123!

---

# 🧱 Funcionalidades do MVP

✔ Login com autenticação via Identity  
✔ Diferenciação de perfil (Instrutor / Aluno)  
✔ Dashboard do instrutor  
✔ Dashboard do aluno  
✔ Cadastro de alunos  
✔ Criação de treinos  
✔ Cadastro de exercícios  
✔ Visualização de treinos do aluno  
✔ Marcação de treino concluído  
✔ Layout responsivo  

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

git clone https://github.com/seuusuario/treinemais.git

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

O sistema será publicado em:

treinemais.lucianoferreiradev.com


Hospedado em **VPS Linux** com:

- Nginx
- .NET Runtime
- SQL Server

---

# 📸 Screenshots

### Dashboard - Admin

![Dashboard Admin](docs/dashboard-admin.png)

### Dashboard - Aluno

![Dashboard Aluno](docs/dashboard-aluno.png)

### Criar Treino

![Criar Treino](docs/criar-treino.png)


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