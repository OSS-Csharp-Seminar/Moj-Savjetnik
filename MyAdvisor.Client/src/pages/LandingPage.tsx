import { Link } from 'react-router-dom'

export default function LandingPage() {
  return (
    <div className="min-h-screen bg-white flex flex-col">
      {/* Navbar */}
      <nav className="flex items-center justify-between px-8 py-4 border-b border-gray-200">
        <span className="text-xl font-semibold text-gray-900">MyAdvisor</span>
        <div className="flex gap-3">
          <Link
            to="/login"
            className="px-4 py-2 text-sm text-gray-700 border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors"
          >
            Log in
          </Link>
          <Link
            to="/register"
            className="px-4 py-2 text-sm text-white bg-indigo-600 rounded-lg hover:bg-indigo-700 transition-colors"
          >
            Get started
          </Link>
        </div>
      </nav>

      {/* Hero */}
      <main className="flex flex-col items-center text-center px-6 pt-24 pb-16 flex-1">
        <h1 className="text-5xl font-bold text-gray-900 leading-tight max-w-2xl">
          Take control of your personal finances
        </h1>
        <p className="mt-6 text-lg text-gray-500 max-w-xl">
          Track spending, set budgets, and get AI-powered insights — all in one place.
        </p>
        <div className="mt-10 flex gap-4">
          <Link
            to="/register"
            className="px-6 py-3 text-white bg-indigo-600 rounded-lg font-medium hover:bg-indigo-700 transition-colors"
          >
            Start for free
          </Link>
          <a
            href="#features"
            className="px-6 py-3 text-gray-700 border border-gray-300 rounded-lg font-medium hover:bg-gray-50 transition-colors"
          >
            Learn more
          </a>
        </div>
      </main>

      {/* Features */}
      <section id="features" className="bg-gray-50 py-20 px-6">
        <h2 className="text-center text-3xl font-bold text-gray-900 mb-12">Everything you need</h2>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-8 max-w-5xl mx-auto">
          {features.map((f) => (
            <div key={f.title} className="bg-white rounded-xl p-6 shadow-sm border border-gray-100">
              <div className="text-3xl mb-4">{f.icon}</div>
              <h3 className="font-semibold text-gray-900 mb-2">{f.title}</h3>
              <p className="text-sm text-gray-500">{f.description}</p>
            </div>
          ))}
        </div>
      </section>

      {/* Footer */}
      <footer className="text-center py-6 text-sm text-gray-400 border-t border-gray-200">
        © {new Date().getFullYear()} MyAdvisor. All rights reserved.
      </footer>
    </div>
  )
}

const features = [
  {
    icon: '📒',
    title: 'Financial Diary',
    description: 'Log every purchase manually and keep a clear record of your daily spending.',
  },
  {
    icon: '📊',
    title: 'Statistics',
    description: 'Visualize your spending over time with easy-to-read charts and breakdowns.',
  },
  {
    icon: '🤖',
    title: 'AI Assistant',
    description: 'Get personalized advice and spending insights from your AI financial advisor.',
  },
]
