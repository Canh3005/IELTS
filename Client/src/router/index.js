import { createRouter, createWebHistory } from "vue-router";
import Home from "../app/Home.vue";
import Register from "../app/Register.vue";
import Login from "../app/Login.vue";
import Progress from "../app/Progress.vue";
import Listening from "../app/Listening.vue";
import Speaking from "../app/Speaking.vue";
import Reading from "../app/Reading.vue";
import Writing from "../app/Writing.vue";
import TestDetail from "../app/TestDetail.vue";
import RenderTest from "../app/RenderTest.vue";
import TestResult from "../app/TestResult.vue";

const routes = [
  { path: "/", component: Home }, // Route cho trang Home
  { path: "/register", component: Register }, // Route cho trang Register
  { path: "/login", component: Login }, // Route cho trang Login
  { path: "/progress", component: Progress }, // Route cho trang Progress
  { path: "/listening", component: Listening, meta: { requiresAuth: true } }, // Route cho trang Listening
  { path: "/speaking", component: Speaking, meta: { requiresAuth: true } }, // Route cho trang Speaking
  { path: "/reading", component: Reading, meta: { requiresAuth: true } }, // Route cho trang Reading
  { path: "/writing", component: Writing, meta: { requiresAuth: true } }, // Route cho trang Writing
  {
    path: "/test/:id",
    component: TestDetail,
    meta: { requiresAuth: true },
    name: "TestDetail",
  }, // Route cho trang TestDetail
  {
    path: "/test/:id/start",
    component: RenderTest,
    meta: { requiresAuth: true },
    name: "RenderTest",
  },
  {
    path: "/result/:id",
    component: TestResult,
    meta: { requiresAuth: true },
    name: "TestResult",
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
