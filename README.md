# 🚀 PortfolioATS - Sistema Completo de Portfolio e Currículo ATS

Sistema backend completo para gerenciamento de portfolio profissional e currículos no formato ATS (Applicant Tracking System), desenvolvido em .NET 9 com MongoDB.

## 📊 Status do Projeto

✅ **Backend Completo** - API RESTful totalmente implementada  
✅ **Autenticação JWT** - Sistema seguro de autenticação  
✅ **Banco de Dados MongoDB** - Modelo de dados otimizado  
✅ **Documentação API** - Swagger/OpenAPI integrado  
✅ **Dashboard Inteligente** - Métricas e insights ATS  
🚧 **Frontend** - Em planejamento

## 🎯 Funcionalidades Principais

### 🔐 Autenticação & Segurança
- Registro e login de usuários
- Autenticação JWT com expiração
- Hash de senha com BCrypt
- Proteção de endpoints com autorização

### 👤 Gerenciamento de Perfil
- Dados pessoais e profissionais
- Resumo profissional otimizado para ATS
- Múltiplas redes sociais (LinkedIn, GitHub, etc.)
- Sistema de progresso e completude do perfil

### 💼 Experiências Profissionais
- Histórico completo de experiências
- Relacionamento com habilidades utilizadas
- Diferentes tipos de contratação (CLT, PJ, Freelance)
- Experiências atuais e passadas

### 🛠️ Habilidades Técnicas
- Categorização por tipo (Backend, Frontend, Cloud, etc.)
- Níveis de proficiência (Básico à Especialista)
- Anos de experiência por skill
- Busca e filtro por categoria

### 🎓 Formação & Certificações
- Histórico acadêmico completo
- Cursos e certificações profissionais
- Controle de validade de certificações
- Organização emissora e credenciais

### 🌐 Idiomas & Competências
- Múltiplos idiomas com níveis de proficiência
- Classificação padrão (Básico, Intermediário, Avançado, etc.)

### 📊 Dashboard Inteligente
- Score ATS para otimização de currículo
- Métricas de completude do perfil
- Sugestões de melhorias
- Estatísticas e atividades recentes

## 🏗️ Arquitetura & Tecnologias

### Backend
- **.NET 9** - Framework principal
- **C# 12** - Linguagem de programação
- **MongoDB** - Banco de dados NoSQL
- **JWT** - Autenticação por tokens
- **BCrypt** - Hash de senhas
- **Swagger** - Documentação automática

### Padrões Arquiteturais
- **Clean Architecture** - Separação de concerns
- **Repository Pattern** - Abstraction de dados
- **DTO Pattern** - Transferência de dados
- **Dependency Injection** - Inversão de controle

### Estrutura do Projeto
PortfolioATS/
├── PortfolioATS.API/ # Camada de apresentação
│ ├── Controllers/ # Endpoints da API
│ ├── Scripts/ # Scripts de inicialização
│ └── Program.cs # Configuração principal
├── PortfolioATS.Core/ # Camada de domínio
│ ├── Entities/ # Entidades de domínio
│ ├── Interfaces/ # Contratos e interfaces
│ ├── DTOs/ # Objetos de transferência
│ └── Models/ # Modelos de configuração
└── PortfolioATS.Infrastructure/ # Camada de infraestrutura
├── Data/ # Contexto do MongoDB
├── Repositories/ # Implementações de repositório
└── Services/ # Serviços de aplicação

## 🚀 Como Executar

### Pré-requisitos
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [MongoDB](https://www.mongodb.com/try/download/community) (local ou Atlas)
- [Git](https://git-scm.com/)

### Configuração do Ambiente

1. **Clonar o repositório**
```bash
git clone https://github.com/seu-usuario/portfolio-ats.git
cd portfolio-ats
```

2. **Configurar banco de dados**
```bash
# MongoDB Local (padrão)
# Connection String: mongodb://localhost:27017
# Database: PortfolioATS_Dev

# Ou MongoDB Atlas
# Editar appsettings.Development.json com sua connection string
```

3. **Restaurar dependências**
```bash
dotnet restore
```

4. **Executar a aplicação**
```bash
dotnet run --project PortfolioATS.API
```

5. **Acessar a aplicação**
- API: https://localhost:7000  
- Swagger UI: https://localhost:7000  
- Health Check: https://localhost:7000/health

## 📚 Documentação da API

### Autenticação
Todos os endpoints (exceto /api/auth/*) requerem autenticação JWT.

Header:
```http
Authorization: Bearer {seu_token_jwt}
```

### 📚 Endpoints da API

---

## 🔐 Autenticação
| Método | Endpoint                  | Descrição             |
|--------|---------------------------|-----------------------|
| POST   | /api/auth/register        | Registrar novo usuário |
| POST   | /api/auth/login           | Login e obter token   |
| POST   | /api/auth/change-password | Alterar senha         |

---

## 📊 Dashboard
| Método | Endpoint                   | Descrição                |
|--------|----------------------------|--------------------------|
| GET    | /api/dashboard             | Dados completos do dashboard |
| GET    | /api/dashboard/completion  | Percentual de completude |
| GET    | /api/dashboard/ats-score   | Score ATS e sugestões    |

---

## 👤 Perfil
| Método | Endpoint       | Descrição           |
|--------|----------------|---------------------|
| GET    | /api/profile   | Obter perfil completo |
| PUT    | /api/profile   | Atualizar perfil    |

---

## 💼 Experiências
| Método | Endpoint                  | Descrição                |
|--------|---------------------------|--------------------------|
| GET    | /api/experiences          | Listar todas experiências |
| GET    | /api/experiences/current  | Experiências atuais      |
| POST   | /api/experiences          | Adicionar experiência    |
| PUT    | /api/experiences/{id}     | Atualizar experiência    |
| DELETE | /api/experiences/{id}     | Remover experiência      |

---

## 🛠️ Habilidades
| Método | Endpoint                        | Descrição             |
|--------|---------------------------------|-----------------------|
| GET    | /api/skills                     | Listar todas habilidades |
| GET    | /api/skills/category/{category} | Skills por categoria  |
| POST   | /api/skills                     | Adicionar skill       |
| PUT    | /api/skills/{id}                | Atualizar skill       |
| DELETE | /api/skills/{id}                | Remover skill         |

---

## 🎓 Formação
| Método | Endpoint               | Descrição          |
|--------|------------------------|--------------------|
| GET    | /api/educations        | Listar formações   |
| POST   | /api/educations        | Adicionar formação |
| PUT    | /api/educations/{id}   | Atualizar formação |
| DELETE | /api/educations/{id}   | Remover formação   |

---

## 📜 Certificações
| Método | Endpoint                        | Descrição                 |
|--------|---------------------------------|---------------------------|
| GET    | /api/certifications             | Listar certificações      |
| GET    | /api/certifications/expired     | Certificações expiradas   |
| POST   | /api/certifications             | Adicionar certificação    |
| PUT    | /api/certifications/{id}        | Atualizar certificação    |
| DELETE | /api/certifications/{id}        | Remover certificação      |

---

## 🌐 Redes Sociais
| Método | Endpoint               | Descrição              |
|--------|------------------------|------------------------|
| GET    | /api/sociallinks       | Listar redes sociais   |
| POST   | /api/sociallinks       | Adicionar rede social  |
| PUT    | /api/sociallinks/{id}  | Atualizar rede social  |
| DELETE | /api/sociallinks/{id}  | Remover rede social    |

---

## 🗣️ Idiomas
| Método | Endpoint                              | Descrição                |
|--------|---------------------------------------|--------------------------|
| GET    | /api/languages                        | Listar idiomas           |
| GET    | /api/languages/proficiency/{level}    | Idiomas por proficiência |
| POST   | /api/languages                        | Adicionar idioma         |
| PUT    | /api/languages/{id}                   | Atualizar idioma         |
| DELETE | /api/languages/{id}                   | Remover idioma           |


## 🗃️ Modelo de Dados

## 📂 Collections Principais

---

### 👤 Users
```json
{
  "_id": "ObjectId",
  "email": "usuario@email.com",
  "passwordHash": "$2a$11$...",
  "role": "User",
  "isActive": true,
  "lastLogin": "2024-01-15T10:30:00Z",
  "profileId": "ObjectId",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-15T10:30:00Z"
}
```
### 📝 Profiles
```json
{
  "_id": "ObjectId",
  "userId": "ObjectId",
  "fullName": "Nome da Pessoa",
  "email": "usuario@email.com",
  "phone": "00 12345-6789",
  "location": "Cidade, UF",
  "professionalSummary": "Resumo Profissional",
  "socialLinks": [
    {
      "id": "guid",
      "platform": "LinkedIn",
      "url": "url do LinkedIn",
      "username": "usuario"
    }
  ],
  "experiences": [
    {
      "id": "guid",
      "company": "Havan Labs",
      "position": "Desenvolvedor .NET Sr",
      "startDate": "2025-01-01T00:00:00Z",
      "endDate": "2025-04-01T00:00:00Z",
      "isCurrent": false,
      "description": "Desenvolvimento e manutenção de aplicações web...",
      "employmentType": "PJ",
      "skillIds": ["skill1", "skill2"],
      "skills": [...]
    }
  ],
  "skills": [
    {
      "id": "guid",
      "name": ".NET",
      "category": "Backend",
      "level": "Avançado",
      "yearsOfExperience": 8
    }
  ],
  "educations": [...],
  "certifications": [...],
  "languages": [...]
}
```
## 🔧 Configuração

## 🌍 Variáveis de Ambiente

Editar `appsettings.Development.json`:

```json
{
  "MongoDBSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "PortfolioATS_Dev"
  },
  "JwtSettings": {
    "SecretKey": "sua-chave-secreta-minimo-32-caracteres",
    "Issuer": "PortfolioATS",
    "Audience": "PortfolioATS-Users",
    "ExpirationMinutes": 1440
  }
}
```
## 🗄️ Índices MongoDB

O sistema cria automaticamente:

🔑 Índice único em Users.Email

🔑 Índice único em Profiles.UserId

📌 Índice em Profiles.Email

## 🧪 Testando a API

## 1. Via Swagger UI
Acesse **https://localhost:7000** para testar interativamente.

---

## 2. Via Postman
Importe a collection **PortfolioATS.postman_collection.json**.

---

## 3. Via curl

```bash
# Registrar usuário
curl -X POST "https://localhost:7000/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"senha123","confirmPassword":"senha123","fullName":"Usuário Teste"}'

# Login
curl -X POST "https://localhost:7000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"senha123"}'

# Obter perfil (com token)
curl -X GET "https://localhost:7000/api/profile" \
  -H "Authorization: Bearer {token}"
```

## 🤝 Contribuição

1. Fork o projeto  
2. Crie uma branch para sua feature  
   ```bash
   git checkout -b feature/AmazingFeature
   ```
3. Commit suas mudanças
   ```bash
   git commit -m "Add some AmazingFeature"
   ```
4. Push para a branch
   ```bash
   git push origin feature/AmazingFeature
   ```
5. Abra um Pull Request

## 📄 Licença
Distribuído sob a licença MIT. Veja LICENSE para mais informações.

## 👨‍💻 Desenvolvido por
Maurício Dias de Carvalho Oliveira

- Email: mauridf@gmail.com  
- LinkedIn: mdcoliveira  
- GitHub: mauridf  

## 🙏 Agradecimentos
- .NET Team pela excelente framework  
- MongoDB pela database flexível  
- Comunidade open source pelas libs utilizadas  
