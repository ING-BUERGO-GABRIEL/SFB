// icons
import {
  QuestionOutlined,
  DashboardOutlined,
  ChromeOutlined,
  LoginOutlined,
  ProfileOutlined,
  FontSizeOutlined,
  BgColorsOutlined,
  BarcodeOutlined,
  CrownOutlined
} from '@ant-design/icons-vue';

// Definición sin tipos TypeScript
const sidebarItem = [
  { header: 'Navigation' },
  {
    title: 'Dashboard',
    icon: DashboardOutlined,
    to: '/dashboard'
  },
  { header: 'Authentication' },
  {
    title: 'Login',
    icon: LoginOutlined,
    to: '/login1'
  },
  {
    title: 'Register',
    icon: ProfileOutlined,
    to: '/register'
  },
  { header: 'Utilities' },
  {
    title: 'Typography',
    icon: FontSizeOutlined,
    to: '/typography'
  },
  {
    title: 'Color',
    icon: BgColorsOutlined,
    to: '/colors'
  },
  {
    title: 'Shadow',
    icon: BarcodeOutlined,
    to: '/shadow'
  },
  {
    title: 'Ant Icons',
    icon: CrownOutlined,
    to: '/icon/ant'
  },
  { header: 'Support' },
  {
    title: 'Sample Page',
    icon: ChromeOutlined,
    to: '/sample-page'
  },
  {
    title: 'Documentation',
    icon: QuestionOutlined,
    to: 'https://codedthemes.gitbook.io/mantis-vuetify/',
    type: 'external',
    chip: 'gitbook',
    chipColor: 'secondary',
    chipVariant: 'flat'
  }
];

export default sidebarItem;
