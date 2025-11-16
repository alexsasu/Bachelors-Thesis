# Bachelors-Thesis

Thesis project made at the final of the Computer Science bachelor program from the Faculty of Mathematics and Computer Science, University of Bucharest, in the year 2023.

## About

The thesis consisted of coming up with a novel set of features for the detection of phishing webpages, training AI models on these features, and showcasing the performance of the best model through a custom web application. It was concluded that only analyzing URL features is not enough for reaching a safe verdict about a web page's status, and that the analysis of HTML and JavaScript features has great significance.

<details>
<summary><h3>AI component</h3></summary>

The data used consisted only of URL strings and was taken from [this GitHub repository](https://github.com/ebubekirbbr/phishing_url_detection). The repository introduced an extensive dataset made up of 5 million URLs gathered from [PhishTank](https://www.phishtank.com/) and [Common Crawl](https://commoncrawl.org/). With 55% legitimate URLs and 45% phishing URLs, it contrasted many earlier datasets which were smaller, imbalanced, and not publicly available. However, due to a lack of computational resources available, only part of the dataset's records were used in this work. As such, the training dataset had 560,000 samples (76% split), the validation dataset had 120,000 samples (16% split), and the test dataset had 60,000 samples (8% split). Each split contained 50% legitimate URLs and 50% phishing URLs.

The main AI models used were XGBoost, MLP, and Random Forest, and they were trained on all of the following features of the analyzed page: URL features, domain features, and HTML and JavaScript features. A CNN model was also used in earlier stages, but it was dropped due to long training times and performance similar to the mentioned models.

The novel aspect of the set of features was that it combined features presented in the following papers from that time: [Phishing Websites Features](https://eprints.hud.ac.uk/id/eprint/24330/6/MohammadPhishing14July2015.pdf), [CANTINA+: A Feature-Rich Machine Learning Framework for Detecting Phishing Web Sites](https://dl.acm.org/doi/abs/10.1145/2019599.2019606), [An effective detection approach for phishing websites using URL and HTML features](https://www.nature.com/articles/s41598-022-10841-5), and [A machine learning based approach for phishing detection using hyperlinks information](https://link.springer.com/article/10.1007/s12652-018-0798-z).

#### Features Used

<details>
<summary>URL features:</summary>

</br>

- character-level tokenization of the URL, with a max sequence length of 200
- number of apparitions of the '.' character
- number of apparitions of the http/https token
- URL length
- URL depth
- apparition of the '@' symbol
- apparition of the '-' inside the domain
- apparition of IP address
- redirect through "//" string
- use of URL shortening service

</details>

<details>
<summary>Domain features:</summary>

</br>

- domain status
- domain age
- domain lifespan
- IP address legitimacy

</details>

<details>
<summary>HTML and JavaScript features:</summary>

</br>

- total number of hyperlinks
- ratio between the number of `<script>` tags containing the `src` attribute and the total number of hyperlinks
- ratio between the number of `<link>` tags containing the `href` attribute and the total number of hyperlinks
- ratio between the number of `<img>` tags containing the `src` attribute and the total number of hyperlinks
- ratio between the number of `<a>` tags containing the `href` attribute and the total number of hyperlinks
- ratio between the number of `<a>` tags without the `href` attribute and the total number of hyperlinks
- ratio between the number of `<script>`, `<link>`, `<img>`, and `<a>` tags that do not contain an URL and the total number of hyperlinks
- ratio between the number of hyperlinks that contain the same domain as the current page's domain and the total number of hyperlinks
- ratio between the number of hyperlinks that contain a different domain than the current page's domain and the total number of hyperlinks
- ratio between the number of hyperlinks that contain the same domain as the current page's domain and the number of hyperlinks that contain a different domain than the current page's domain
- total number of `<form>` tags
- ratio between the number of suspicious `<form>` tags and the total number of `<form>` tags
- number of redirects
- percentage of objects in `<img>`, `<audio>`, `<embed>`, and `<iframe>` tags with foreign domains
- percentage of suspicious `<a>` tags
- percentage of suspicious `<link>` and `<script>` tags
- sending e-mails through forms
- dimension and border of `<iframe>` tags
- foreign favicon domain

</details>

In total, 232 features were used.

#### Main Results

| Model | Training Acc. | Validation Acc. | Test Acc. |
|:---:|---:|---:|---:|
| XGBoost | 93.7% | **93.5%** | 93.5% |
| MLP | 96.0% | 92.9% | - |
| Random Forest | 92.6% | 92.0% | - |

Given these results, the XGBoost model was chosen for integration into the web application.

<details>
<summary>In addition, upon analyzing the importance of all features minus the 200 generated by the tokenizer during the training of the chosen model, it was remarked that, in total, they only amounted to 25.79% of the total importance, with the rest belonging to the tokenized URL:</summary>

<img width="482.5" height="455" alt="xgb_feature_importances (cel mai ligibil format)" src="https://github.com/user-attachments/assets/31e3e26f-2f6a-407b-941c-6f8262b72308"/>

</details>

#### Technologies Used

The following main Python libraries, packages, and modules were used: TensorFlow, Keras, Scikit-learn, Xgboost, Requests, BeautifulSoup, Urllib, Whois, Re, NumPy, Pandas.

#### References

Part of the provided code was taken from the GitHub repositories [Malicious-Web-Content-Detection-Using-Machine-Learning](https://github.com/philomathic-guy/Malicious-Web-Content-Detection-Using-Machine-Learning) and [Phishing-Website-Detection-by-Machine-Learning-Techniques](https://github.com/shreyagopal/Phishing-Website-Detection-by-Machine-Learning-Techniques).

</details>

<details>
<summary><h3>Backend component:</h3></summary>

The application's backend was programmed using the ASP.NET Core framework. It allows for user account creation and management, and URL report generation and retrieval from the database. It makes use of several good practices, such as exception handling, separation of concerns through the Onion architecture, and role-based endpoint authorization.

Backend structured through the Onion architecture ([source](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)):

<img width="687.5" height="332.5" alt="image5-7" src="https://github.com/user-attachments/assets/d6b79732-7793-4c17-bd1e-10e101dc3627" />

The database was managed through the SQL Server Management Studio software.

#### Technologies Used

The principal technologies used were the following: Entity Framework Core, ASP.NET Core Identity, JWT Bearer, Whois library.

</details>

<details>
<summary><h3>Frontend component:</h3></summary>

The project's UI was built using the Angular framework and stylized with the help of the Bootstrap framework, comprising of five web pages: "Home", where an (un)authenticated user can specify a web page URL and obtain a report of it after a short while, "URL Reports", where each user's web page reports are displayed, "Profile", "Register", and "Login".

Concerning security aspects, an interceptor was used to log out the user if the HTTP status code of any request received was 401, 403 or 500, and an auth guard was used to redirect the user to the "Login" page if they tried to access the pages "URL Reports" or "Profile" while being unauthenticated.

</details>

<details>
<summary><h3>Presentation of the web application:</h3></summary>

![chrome_3X7GzsL3kI](https://github.com/user-attachments/assets/e189fce5-e2b5-4c0a-96a1-3a29ae48a549)

</details>
