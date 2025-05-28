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
import CreateWritingTest from "../app/CreateWritingTest.vue";
import CreateSpeakingTest from "../app/CreateSpeakingTest.vue";
import CreateListeningTest from "../app/CreateListeningTest.vue";
import CreateReadingTest from "../app/CreateReadingTest.vue";

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
  {
    path: "/create-writing-test",
    name: "CreateWritingTest",
    component: CreateWritingTest,
    meta: { requiresAuth: true },
  },
  // Route cho trang CreateWritingTest
  {
    path: "/create-speaking-test",
    name: "CreateSpeakingTest",
    component: CreateSpeakingTest,
    meta: { requiresAuth: true },
  },
  // Route cho trang CreateSpeakingTest
  {
    path: "/create-listening-test",
    name: "CreateListeningTest",
    component: CreateListeningTest,
    meta: { requiresAuth: true },
  },
  // Route cho trang CreateListeningTest
  {
    path: "/create-reading-test",
    name: "CreateReadingTest",
    component: CreateReadingTest,
    meta: { requiresAuth: true },
  },
  // Route cho trang CreateReadingTest
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
