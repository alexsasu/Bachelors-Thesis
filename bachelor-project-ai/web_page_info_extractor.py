from bs4 import BeautifulSoup as bs
from datetime import datetime
import json
import re
import requests
import socket
from urllib.parse import urlparse
from tensorflow.keras.preprocessing.text import Tokenizer
from tensorflow.keras.preprocessing.sequence import pad_sequences
import whois


def tokenize_url(url):
    tokenizer = Tokenizer(lower=False, char_level=True, oov_token="UNK")
    with open(r"C:\Users\Alex\Desktop\Diverse\GitHub\Repos\Bachelor-Thesis\bachelor-project-ai\tokenizer_dict.json") as json_file:
        tokenizer.word_index = json.loads(json_file.read())

    url_tokenized = tokenizer.texts_to_sequences([url])
    max_len_padded_seq = 200
    url_tokenized = pad_sequences(url_tokenized, maxlen=max_len_padded_seq, padding='post', truncating='post')

    return list(url_tokenized[0])


def dots_status(url):
    count = url.count('.')
    if count in [1, 2]:
        return 0
    elif count == 3:
        return 1
    return 2


def http_tokens_status(url):
    tokens = re.findall(r"http[s]?:|http[s]?\.|http[s]?-|http[s]?_", url)
    if len(tokens) != 1:
        return 2
    return 0


def url_length(url):
    if len(url) < 54:
        return 0
    elif 54 <= len(url) <= 80:
        return 1
    return 2


def having_at_symbol(url):
    if '@' in url:
        return 2
    return 0


def having_prefix_suffix(hostname):
    if '-' in hostname:
        return 2
    return 0


def having_ip_address(url):
    match_ipv4_dec = re.search("([01]?\\d\\d?|2[0-4]\\d|25[0-5])(\\.|\\-)([01]?\\d\\d?|2[0-4]\\d|25[0-5])(\\.|\\-)([01]?\\d\\d?|2[0-4]\\d|25[0-5])(\\.|\\-)([01]?\\d\\d?|2[0-4]\\d|25[0-5])", url)
    match_ipv4_hex = re.search("(0x[0-9a-fA-F]{1,2})(\\.|\\-)(0x[0-9a-fA-F]{1,2})(\\.|\\-)(0x[0-9a-fA-F]{1,2})(\\.|\\-)(0x[0-9a-fA-F]{1,2})", url)
    match_ipv6 = re.search("(?:[a-fA-F0-9]{1,4}:){7}[a-fA-F0-9]{1,4}", url)
    if match_ipv4_dec or match_ipv4_hex or match_ipv6:
        return 2
    return 0


def double_slash_redirecting(url):
    double_slash_pos = url.rfind("//")
    if double_slash_pos > 6:
        return 2
    return 0


def using_shortening_services(url):
    shortening_services = r"bit\.ly|goo\.gl|shorte\.st|go2l\.ink|x\.co|ow\.ly|t\.co|tinyurl|tr\.im|is\.gd|cli\.gs|" \
                          r"yfrog\.com|migre\.me|ff\.im|tiny\.cc|url4\.eu|twit\.ac|su\.pr|twurl\.nl|snipurl\.com|" \
                          r"short\.to|BudURL\.com|ping\.fm|post\.ly|Just\.as|bkite\.com|snipr\.com|fic\.kr|loopt\.us|" \
                          r"doiop\.com|short\.ie|kl\.am|wp\.me|rubyurl\.com|om\.ly|to\.ly|bit\.do|t\.co|lnkd\.in|db\.tt|" \
                          r"qr\.ae|adf\.ly|goo\.gl|bitly\.com|cur\.lv|tinyurl\.com|ow\.ly|bit\.ly|ity\.im|q\.gs|is\.gd|" \
                          r"po\.st|bc\.vc|twitthis\.com|u\.to|j\.mp|buzurl\.com|cutt\.us|u\.bb|yourls\.org|x\.co|" \
                          r"prettylinkpro\.com|scrnch\.me|filoops\.info|vzturl\.com|qr\.net|1url\.com|tweez\.me|v\.gd|" \
                          r"tr\.im|link\.zip\.net"
    shortening_service = re.search(shortening_services, url)
    if shortening_service:
        return 2
    return 0


def domain_age(domain):
    creation_date = domain.creation_date
    expiration_date = domain.expiration_date
    try:
        age_of_domain = abs((expiration_date - creation_date).days)
    except:
        return 1
    if age_of_domain / 30 < 6:
        return 2
    return 0


def domain_remaining_registration(domain):
    today = datetime.now()
    expiration_date = domain.expiration_date
    try:
        remaining_registration = abs((expiration_date - today).days)
    except:
        return 1
    if remaining_registration / 30 < 6:
        return 2
    return 0


def statistical_report(url, hostname):
    try:
        ip_address = socket.gethostbyname(hostname)
    except:
        return 2
    match_url = re.search(
        r"at\.ua|usa\.cc|baltazarpresentes\.com\.br|pe\.hu|esy\.es|hol\.es|sweddy\.com|myjino\.ru|96\.lt|ow\.ly", url)
    match_ip = re.search(
        "146\.112\.61\.108|213\.174\.157\.151|121\.50\.168\.88|192\.185\.217\.116|78\.46\.211\.158|181\.174\.165\.13|46\.242\.145\.103|121\.50\.168\.40|83\.125\.22\.219|46\.242\.145\.98|"
        "107\.151\.148\.44|107\.151\.148\.107|64\.70\.19\.203|199\.184\.144\.27|107\.151\.148\.108|107\.151\.148\.109|119\.28\.52\.61|54\.83\.43\.69|52\.69\.166\.231|216\.58\.192\.225|"
        "118\.184\.25\.86|67\.208\.74\.71|23\.253\.126\.58|104\.239\.157\.210|175\.126\.123\.219|141\.8\.224\.221|10\.10\.10\.10|43\.229\.108\.32|103\.232\.215\.140|69\.172\.201\.153|"
        "216\.218\.185\.162|54\.225\.104\.146|103\.243\.24\.98|199\.59\.243\.120|31\.170\.160\.61|213\.19\.128\.77|62\.113\.226\.131|208\.100\.26\.234|195\.16\.127\.102|195\.16\.127\.157|"
        "34\.196\.13\.28|103\.224\.212\.222|172\.217\.4\.225|54\.72\.9\.51|192\.64\.147\.141|198\.200\.56\.183|23\.253\.164\.103|52\.48\.191\.26|52\.214\.197\.72|87\.98\.255\.18|209\.99\.17\.27|"
        "216\.38\.62\.18|104\.130\.124\.96|47\.89\.58\.141|78\.46\.211\.158|54\.86\.225\.156|54\.82\.156\.19|37\.157\.192\.102|204\.11\.56\.48|110\.34\.231\.42",
        ip_address)
    if match_url or match_ip:
        return 2
    return 0


def get_total_hyperlinks(soup):
    total_hyperlinks = 0

    for a in soup.find_all("a"):
        total_hyperlinks += 1
    for img in soup.find_all("img"):
        total_hyperlinks += 1
    for link in soup.find_all("link"):
        total_hyperlinks += 1
    for script in soup.find_all("script"):
        total_hyperlinks += 1

    return total_hyperlinks


def a_tags_rate(soup, total_hyperlinks):
    tags = 0
    for a in soup.find_all("a", href=True):
        tags += 1
    if total_hyperlinks > 0:
        return tags / total_hyperlinks
    return 0


def img_tags_rate(soup, total_hyperlinks):
    tags = 0
    for img in soup.find_all("img", src=True):
        tags += 1
    if total_hyperlinks > 0:
        return tags / total_hyperlinks
    return 0


def link_tags_rate(soup, total_hyperlinks):
    tags = 0
    for link in soup.find_all("link", href=True):
        tags += 1
    if total_hyperlinks > 0:
        return tags / total_hyperlinks
    return 0


def script_tags_rate(soup, total_hyperlinks):
    tags = 0
    for script in soup.find_all("script", src=True):
        tags += 1
    if total_hyperlinks > 0:
        return tags / total_hyperlinks
    return 0


def a_tags_without_href_rate(soup, total_hyperlinks):
    tags = 0
    for a in soup.find_all("a", href=False):
        tags += 1
    if total_hyperlinks > 0:
        return tags / total_hyperlinks
    return 0


def empty_hyperlinks_rate(soup, total_hyperlinks):
    tags = 0

    for a in soup.find_all("a", href=True):
        if "#" in a["href"] or "javascript" in a["href"].lower() or a["href"] == "":
            tags += 1
    for img in soup.find_all("img", src=True):
        if "#" in img["src"] or "javascript" in img["src"].lower() or img["src"] == "":
            tags += 1
    for link in soup.find_all("link", href=True):
        if "#" in link["href"] or "javascript" in link["href"].lower() or link["href"] == "":
            tags += 1
    for script in soup.find_all("script", src=True):
        if "#" in script["src"] or "javascript" in script["src"].lower() or script["src"] == "":
            tags += 1

    if total_hyperlinks > 0:
        return tags / total_hyperlinks
    return 0


def internal_and_external_hyperlinks_info(soup, url, hostname, domain_name, total_hyperlinks):
    internal_hyperlinks, external_hyperlinks = 0, 0

    if domain_name is None:
        domain_name = ""

    for a in soup.find_all("a", href=True):
        if url.lower() in a["href"].lower() or hostname in a["href"].lower() or domain_name in a["href"].lower() or domain_name.split(".")[0] in a["href"].lower():
            internal_hyperlinks += 1
        elif "http" in a["href"]:
            external_hyperlinks += 1
    for img in soup.find_all("img", src=True):
        if url.lower() in img["src"].lower() or hostname in img["src"].lower() or domain_name in img["src"].lower() or domain_name.split(".")[0] in img["src"].lower():
            internal_hyperlinks += 1
        elif "http" in img["src"]:
            external_hyperlinks += 1
    for link in soup.find_all("link", href=True):
        if url.lower() in link["href"].lower() or hostname in link["href"].lower() or domain_name in link["href"].lower() or domain_name.split(".")[0] in link["href"].lower():
            internal_hyperlinks += 1
        elif "http" in link["href"]:
            external_hyperlinks += 1
    for script in soup.find_all("script", src=True):
        if url.lower() in script["src"].lower() or hostname in script["src"].lower() or domain_name in script["src"].lower() or domain_name.split(".")[0] in script["src"].lower():
            internal_hyperlinks += 1
        elif "http" in script["src"]:
            external_hyperlinks += 1

    f_int, f_ext, f_ext_int = 0, 0, 0
    if total_hyperlinks > 0:
        f_int, f_ext = internal_hyperlinks / total_hyperlinks, external_hyperlinks / total_hyperlinks
    if internal_hyperlinks > 0:
        f_ext_int = external_hyperlinks / internal_hyperlinks

    return [f_int, f_ext, f_ext_int]


def forms_info(soup, url, hostname, domain_name, unknown_token):
    total_forms, suspicious_forms = 0, 0

    if domain_name is None:
        domain_name = unknown_token

    for form in soup.find_all("form"):
        total_forms += 1
        try:
            if form["action"] == "" or (url.lower() not in form["action"].lower() and hostname not in form["action"].lower() and domain_name not in form["action"].lower() and domain_name.split(".")[0] not in form["action"].lower()):
                suspicious_forms += 1
        except:
            pass

    return [total_forms, 0 if total_forms == 0 else suspicious_forms / total_forms]


def redirects_status(response):
    if len(response.history) <= 1:
        return 0
    elif 2 <= len(response.history) <= 3:
        return 1
    return 2


def request_foreign_url(url, hostname, domain_name, soup):
    tags = 0
    appearances = 0
    for img in soup.find_all("img", src=True):
        tags += 1
        if domain_name is not None:
            if url.lower() not in img["src"].lower() and hostname not in img["src"].lower() and domain_name not in img["src"].lower() and domain_name.split(".")[0] not in img["src"].lower():
                appearances += 1
        else:
            if url.lower() not in img["src"].lower() and hostname not in img["src"].lower():
                appearances += 1
    for audio in soup.find_all("audio", src=True):
        tags += 1
        if domain_name is not None:
            if url.lower() not in audio["src"].lower() and hostname not in audio["src"].lower() and domain_name not in audio["src"].lower() and domain_name.split(".")[0] not in audio["src"].lower():
                appearances += 1
        else:
            if url.lower() not in audio["src"].lower() and hostname not in audio["src"].lower():
                appearances += 1
    for embed in soup.find_all("embed", src=True):
        tags += 1
        if domain_name is not None:
            if url.lower() not in embed["src"].lower() and hostname not in embed["src"].lower() and domain_name not in embed["src"].lower() and domain_name.split(".")[0] not in embed["src"].lower():
                appearances += 1
        else:
            if url.lower() not in embed["src"].lower() and hostname not in embed["src"].lower():
                appearances += 1
    for i_frame in soup.find_all("i_frame", src=True):
        tags += 1
        if domain_name is not None:
            if url.lower() not in i_frame["src"].lower() and hostname not in i_frame["src"].lower() and domain_name not in i_frame["src"].lower() and domain_name.split(".")[0] not in i_frame["src"].lower():
                appearances += 1
        else:
            if url.lower() not in i_frame["src"].lower() and hostname not in i_frame["src"].lower():
                appearances += 1

    try:
        percentage = appearances / float(tags) * 100
    except:
        return 0

    if percentage < 22.0:
        return 0
    elif 22.0 <= percentage <= 61.0:
        return 1
    return 2


def suspicious_anchors(url, hostname, domain_name, soup):
    tags = 0
    appearances = 0
    for a in soup.find_all("a", href=True):
        tags += 1
        if domain_name is not None:
            if "javascript" in a["href"].lower() or "mailto" in a["href"].lower() or not (url.lower() in a["href"].lower() or hostname in a["href"].lower() or domain_name in a["href"].lower() or domain_name.split(".")[0] in a["href"].lower()):
                appearances += 1
        else:
            if "javascript" in a["href"].lower() or "mailto" in a["href"].lower() or not (url.lower() in a["href"].lower() or hostname in a["href"].lower()):
                appearances += 1

    try:
        percentage = appearances / float(tags) * 100
    except:
        return 0

    if percentage < 31.0:
        return 0
    elif 31.0 <= percentage <= 67.0:
        return 1
    return 2


def foreign_links_in_tags(url, hostname, domain_name, soup):
    tags = 0
    appearances = 0
    for link in soup.find_all("link", href=True):
        tags += 1
        if domain_name is not None:
            if url.lower() not in link["href"].lower() and hostname not in link["href"].lower() and domain_name not in link["href"].lower() and domain_name.split(".")[0] not in link["href"].lower():
                appearances += 1
        else:
            if url.lower() not in link["href"].lower() and hostname not in link["href"].lower():
                appearances += 1
    for script in soup.find_all("script", src=True):
        tags += 1
        if domain_name is not None:
            if url.lower() not in script["src"].lower() and hostname not in script["src"].lower() and domain_name not in script["src"].lower() and domain_name.split(".")[0] not in script["src"].lower():
                appearances += 1
        else:
            if url.lower() not in script["src"].lower() and hostname not in script["src"].lower():
                appearances += 1

    try:
        percentage = appearances / float(tags) * 100
    except:
        return 0

    if percentage < 17.0:
        return 0
    elif 17.0 <= percentage <= 81.0:
        return 1
    return 2


def email_in_forms(soup):
    for form in soup.find_all("form", action=True):
        if "mailto" in form["action"].lower():
            return 2
    return 0


def sfh_status(url, hostname, domain_name, soup):
    for form in soup.find_all("form", action=True):
        if form["action"] == "" or form["action"].lower() == "about:blank":
            return 2
        else:
            if domain_name is not None:
                if url.lower() not in form["action"].lower() and hostname not in form["action"].lower() and domain_name not in form["action"].lower() and domain_name.split(".")[0] not in form["action"].lower():
                    return 1
            else:
                if url.lower() not in form["action"].lower() and hostname not in form["action"].lower():
                    return 1
    return 0


def having_iframe(soup):
    for i_frame in soup.find_all("i_frame", width=True, height=True, frameBorder=True):
        if i_frame["width"] == '0' and i_frame["height"] == '0' and i_frame["frameBorder"] == '0':
            return 2
        if i_frame["width"] == '0' or i_frame["height"] == '0' or i_frame["frameBorder"] == '0':
            return 1
    return 0


def favicon_info(url, hostname, domain_name, soup):
    for head in soup.find_all("head"):
        for head.link in soup.find_all("link", href=True):
            if domain_name is not None:
                if url.lower() not in head.link["href"].lower() and hostname not in head.link["href"].lower() and domain_name not in head.link["href"].lower() and domain_name.split(".")[0] not in head.link["href"].lower():
                    return 2
            else:
                if url.lower() not in head.link["href"].lower() and hostname not in head.link["href"].lower():
                    return 2
    return 0


def main(url):
    features = []
    unknown_token = "?_---UNKNOWN---_?"

    hostname = urlparse(url).hostname
    hostname = unknown_token if not isinstance(hostname, str) else hostname.lower()

    # URL features
    features.extend(tokenize_url(url))
    features.append(dots_status(url))
    features.append(http_tokens_status(url))
    features.append(url_length(url))
    features.append(having_at_symbol(url))
    features.append(2 if hostname == unknown_token else having_prefix_suffix(hostname))
    ip_address_status = having_ip_address(url)
    features.append(ip_address_status)
    features.append(double_slash_redirecting(url))
    features.append(using_shortening_services(url))

    # Web page domain features
    domain_name = None
    try:
        domain = whois.whois(hostname)
        domain_name = domain.domain_name
        if isinstance(domain_name, str):
            domain_name = domain_name.lower()
        elif isinstance(domain_name, list) or isinstance(domain_name, tuple):
            domain_name = domain_name[0].lower()
        else:
            domain_name = None
        domain_status = 0
    except:
        domain_status = 2
    features.append(domain_status)
    features.append(2 if domain_status == 2 else domain_age(domain))
    features.append(2 if domain_status == 2 else domain_remaining_registration(domain))
    features.append(statistical_report(url, hostname))

    # HTML and JS features
    response = ""
    total_hyperlinks = 0
    if ip_address_status != 2:
        try:
            response = requests.get(url, timeout=5)
            content = response.content
            soup = bs(content, "html.parser")
            total_hyperlinks = get_total_hyperlinks(soup)
        except:
            pass
    features.append(2 if response == "" or ip_address_status == 2 else total_hyperlinks)
    features.append(2 if response == "" or ip_address_status == 2 else a_tags_rate(soup, total_hyperlinks))
    features.append(2 if response == "" or ip_address_status == 2 else img_tags_rate(soup, total_hyperlinks))
    features.append(2 if response == "" or ip_address_status == 2 else link_tags_rate(soup, total_hyperlinks))
    features.append(2 if response == "" or ip_address_status == 2 else script_tags_rate(soup, total_hyperlinks))
    features.append(2 if response == "" or ip_address_status == 2 else a_tags_without_href_rate(soup, total_hyperlinks))
    features.append(2 if response == "" or ip_address_status == 2 else empty_hyperlinks_rate(soup, total_hyperlinks))
    features.extend([2, 2, 2] if response == "" or ip_address_status == 2 else internal_and_external_hyperlinks_info(soup, url, hostname, domain_name, total_hyperlinks))
    features.extend([2, 2] if response == "" or ip_address_status == 2 else forms_info(soup, url, hostname, domain_name, unknown_token))
    features.append(2 if response == "" or ip_address_status == 2 else redirects_status(response))
    features.append(2 if response == "" or ip_address_status == 2 else request_foreign_url(url, hostname, domain_name, soup))
    features.append(2 if response == "" or ip_address_status == 2 else suspicious_anchors(url, hostname, domain_name, soup))
    features.append(2 if response == "" or ip_address_status == 2 else foreign_links_in_tags(url, hostname, domain_name, soup))
    features.append(2 if response == "" or ip_address_status == 2 else email_in_forms(soup))
    features.append(2 if response == "" or ip_address_status == 2 else sfh_status(url, hostname, domain_name, soup))
    features.append(2 if response == "" or ip_address_status == 2 else having_iframe(soup))
    features.append(2 if response == "" or ip_address_status == 2 else favicon_info(url, hostname, domain_name, soup))

    return features, domain_name
