import { useState } from "react";
import "../app/globals.css";

export default function ScrapePage() {
  const [url, setUrl] = useState("");
  const [data, setData] = useState<any>({
    Url: "",
    Images: [],
    Links: { Internal: [], External: [] },
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const fetchData = async (requestedUrl: string) => {
    setLoading(true);
    setError("");

    console.log("Sending request to API with URL:", requestedUrl);

    try {
      const response = await fetch(
        "https://localhost:7215/api/WebScraper/scrape",
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({ url: requestedUrl }),
        }
      );

      if (!response.ok) throw new Error("Failed to fetch data");

      const result = await response.json();
      console.log("API Response:", result);

      setData({
        Url: result.url,
        Images: result.images,
        Links: {
          Internal: result.links.internal,
          External: result.links.external,
        },
      });
    } catch (err) {
      console.error("Fetch error:", err);
      setError("Failed to fetch data. Please check the URL and try again.");
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await fetchData(url);
  };

  console.log("Rendered data:", data);

  return (
    <div className="grid grid-rows-[auto_1fr_auto] items-center justify-items-center min-h-screen p-8 pb-20 gap-8 sm:p-20">
      <main className="flex flex-col gap-6 row-start-2 items-center w-full max-w-lg">
        <h1 className="text-3xl font-bold text-gray-900 dark:text-gray-100">
          Web Scraper
        </h1>

        {/* فرم ورود URL */}
        <form
          onSubmit={handleSubmit}
          className="w-full flex flex-col gap-4 p-6 bg-white shadow-md rounded-lg"
        >
          <label className="text-gray-700 font-medium">Enter a URL:</label>
          <input
            type="text"
            value={url}
            onChange={(e) => setUrl(e.target.value)}
            className="w-full p-3 border border-gray-300 rounded-lg"
            placeholder="https://example.com"
            required
          />
          <button
            type="submit"
            className="w-full bg-blue-500 text-white py-2 rounded-lg text-lg hover:bg-blue-600"
          >
            {loading ? "Scraping..." : "Scrape"}
          </button>
        </form>

        {error && <p className="text-red-500 mt-2">{error}</p>}

        {data?.Url && (
          <div className="w-full max-w-lg p-6 bg-white shadow-md rounded-lg">
            <h2 className="text-2xl font-semibold mb-3 text-gray-900 dark:text-gray-100">
              Results:
            </h2>
            <p className="text-gray-700">
              Extracted from: <span className="font-semibold">{data.Url}</span>
            </p>

            <h3 className="text-lg font-semibold mt-4 text-gray-900 dark:text-gray-100">
              Images:
            </h3>
            <ul className="list-disc pl-5">
              {data?.Images?.length > 0 &&
                data.Images.map((img: any, index: number) => (
                  <li key={index} className="text-blue-600 hover:underline">
                    <a href={img.url} target="_blank" rel="noopener noreferrer">
                      {img.url} ({img.size} bytes)
                    </a>
                  </li>
                ))}
            </ul>

            <h3 className="text-lg font-semibold mt-4 text-gray-900 dark:text-gray-100">
              Links:
            </h3>

            <h4 className="font-medium text-gray-900 dark:text-gray-100">
              Internal Links:
            </h4>
            <ul className="list-disc pl-5 overflow-auto max-h-40 border border-gray-300 p-2 rounded-lg">
              {data?.Links?.Internal?.length > 0 &&
                data.Links.Internal.map((link: string, index: number) => (
                  <li key={index}>
                    <button
                      onClick={() => fetchData(link)}
                      className="text-blue-600 underline hover:text-blue-800"
                    >
                      {link}
                    </button>
                  </li>
                ))}
            </ul>

            <h4 className="font-medium mt-2 text-gray-900 dark:text-gray-100">
              External Links:
            </h4>
            <ul className="list-disc pl-5 overflow-auto max-h-40 border border-gray-300 p-2 rounded-lg">
              {data?.Links?.External?.length > 0 &&
                data.Links.External.map((link: string, index: number) => (
                  <li key={index}>
                    <button
                      onClick={() => fetchData(link)}
                      className="text-blue-600 underline hover:text-blue-800"
                    >
                      {link}
                    </button>
                  </li>
                ))}
            </ul>
          </div>
        )}
      </main>
    </div>
  );
}
