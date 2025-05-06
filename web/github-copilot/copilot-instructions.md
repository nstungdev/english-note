# GitHub Copilot Instruction Guide

This document outlines a set of rules that GitHub Copilot must follow when generating or suggesting code for this repository. These rules ensure consistency, readability, and alignment with the project's goals.

## 📌 General Rules

1. **Language Consistency**  
   All generated text, including comments, variable names, and documentation, must be written in **English** and stay **relevant to the context of English language learning**.

2. **File and Folder Structure**  
   When creating new files or folders, **refer to** the `CLEAN_ARCHITECTURE_STRUCTURE.md`.

3. **Code Readability**  
   Code should be **clearly structured**. Each function **must not exceed 50 lines** to ensure readability and maintainability.

4. **Constants Naming Convention**  
   All constants must be written in **uppercase with words separated by underscores** (e.g., `ACCESS_TOKEN`).

5. **Avoid Raw Strings**  
   Repeated string literals should **not be hardcoded**. Instead, store them in a central constants file or module.

6. **Clear Variable Names**  
   Variable names must be **clear and descriptive**, reflecting their purpose and usage.

7. **Statement Termination**  
   Every statement must end with a **semicolon (`;`)** to maintain consistency and reduce potential syntax errors.

---

By adhering to these rules, we ensure a clean, maintainable, and scalable codebase that aligns with both technical and educational goals.
