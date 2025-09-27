# 📌 Resources Platform  

Welcome team 👋  
This repository contains all modules of the platform (API – Web – Mobile).  
Please follow the rules below to keep our codebase clean, consistent, and collaborative.  

---

## 🚀 Project Structure  
- **/api** → Contains API projects (Core, Infrastructure, Services, APIs).  
- **/web** → Contains the web application.  
- **/mobile** → Contains the mobile applications.  

---

## 🌱 Branching Strategy  
- **main** → Stable code (production-ready).  
- **dev** → Main development branch (❌ no direct pushes allowed).  
- **feature/** → For each new feature or fix.  

### Examples:  
- `feature/api-CardController`  
- `feature/web-LoginPage`  
- `feature/mobile-ProfileScreen`  

After finishing a feature branch → open a **Pull Request (PR)** into `dev`.  

---

## ✍️ Commit Rules  
Commit messages must be clear and follow a convention:  

### Examples:  
- `feat(api): add CardController`  
- `fix(web): resolve login bug`  
- `chore(mobile): update dependencies`  

**Allowed types:**  
- `feat` → New feature.  
- `fix` → Bug fix.  
- `refactor` → Code improvement without behavior change.  
- `docs` → Documentation changes.  
- `chore` → Config, libraries, scripts.  

