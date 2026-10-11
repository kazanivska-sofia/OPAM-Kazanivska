N = 11, K = 22, a = 2, b = 3, c = 1

# Лабораторна робота №3. Рекурсивні алгоритми

**Виконавець:** Софія  
**Варіант:** N=11, K=22  

---

## 1. Базові випадки рекурсивних функцій

* **Завдання 1:** Базовим випадком є каталог, який не містить підкаталогів (цикл по підпапках не виконує жодної ітерації).
* **Завдання 2:** Базовим випадком є підмасив з одного елемента (`lo >= hi`), для якого перевірка повертає `true`.
* **Завдання 3:** Базовим випадком є досягнення цільової суми (`target == 0`) або перевищення межі/індексу (`target < 0` чи `index >= arr.Length`).

---

## 2. Трасування (Завдання 4)

```text
enter AllEqual(0..3)
  enter AllEqual(0..1)
    enter AllEqual(0..0)
    exit  AllEqual(0..0) -> True
    enter AllEqual(1..1)
    exit  AllEqual(1..1) -> True
  exit  AllEqual(0..1) -> True
  enter AllEqual(2..3)
    enter AllEqual(2..2)
    exit  AllEqual(2..2) -> True
    enter AllEqual(3..3)
    exit  AllEqual(3..3) -> True
  exit  AllEqual(2..3) -> True
exit  AllEqual(0..3) -> True