// src/pages/Recommendations.tsx
import React from 'react';
import styles from './Recommendations.module.css';

// ---------- Елементи рекомендацій з емодзі ---------- //
const recommendations = [
  {
    id: 1,
    icon: '🚜',
    title: 'Ефективність фарму',
    description: 'Відстежуйте точність останніх ударів і намагайтеся підтримувати щонайменше 70% на лінії. Чередуйте крипів на лінії та в лісі, щоб максимізувати золото за хвилину.',
  },
  {
    id: 2,
    icon: '👁️',
    title: 'Обізнаність карти та візія',
    description: 'Розміщуйте варди в зонах із високим трафіком кожні 2–3 хвилини. Відстежуйте кількість переглядів мінімапи за хвилину: прагніть щонайменше 2 переглядів на хвилину.',
  },
  {
    id: 3,
    icon: '⚔️',
    title: 'Позиціювання в командних боях',
    description: 'Переглядайте реплеї, щоб помітити своє позиціювання відносно ворогів. Тримайтесь максимальної дистанції застосування здібностей і використовуйте туман війни на свою користь.',
  },
  {
    id: 4,
    icon: '🎯',
    title: 'Практика останніх ударів',
    description: 'Використовуйте тренувальне лобі тільки з дальніми крипами. Зосередьтесь на таймінгу атаки та нанесенні шкоди, щоб отримувати стабільні останні удари.',
  },
  {
    id: 5,
    icon: '🏆',
    title: 'Визначення пріоритетів цілей',
    description: 'Після вбивств віддавайте пріоритет руйнуванню веж та Рошану. Відстежуйте шкоду по вежах за хвилину і прагніть підвищити її на 10%.',
  }
];

export default function Recommendations() {
  return (
    <div className={styles.container}>
      <h2 className={styles.heading}>Рекомендації для покращення навичок</h2>
      <div className={styles.list}>
        {recommendations.map(rec => (
          <div key={rec.id} className={styles.card}>
            <div className={styles.iconWrapper}>
              <span className={styles.icon}>{rec.icon}</span>
            </div>
            <div className={styles.content}>
              <h3 className={styles.title}>{rec.title}</h3>
              <p className={styles.description}>{rec.description}</p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
