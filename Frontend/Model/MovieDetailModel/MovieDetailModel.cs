using Frontend.Entities;
using Frontend.Service;

namespace Frontend.Model.MovieDetailModel;

public class MovieDetailModel: IMovieDetailModel
{
    private const string BASEURI = "http://localhost:5276";
    public async Task<Movie> GetMovieDetails(string movieId)
    {
        // List<Actor> actors = new List<Actor>();
        // actors.Add(new Actor{ID = 1, Name = "Solaiman", BirthYear = 1991});
        // actors.Add(new Actor{ID = 2, Name = "Bastian", BirthYear = 1992});
        // List<Director> directors = new List<Director>();
        // directors.Add(new Director{ID = 1, Name = "Solaiman"});
        // Rating rating = new Rating { AverageRating = 7, RatingCount = 10 };
        // Movie movie = new Movie
        // {
        //     Id = movieId,
        //     Title = "Blabla",
        //     ReleaseYear = 2000,
        //     Actors = actors,
        //     Directors = directors,
        //     Rating = rating,
        //     PosterUrl = new Uri("data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAoHCBYVFRgWFhYZGBgaHSQaHRwaHB8hHBwcHh4eHRweGhweIS4lHB8rIR4cJjgmKy8xNTU1HCQ7QDs0Py40NTEBDAwMEA8QHhISHzYsJSs0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NDQ0NP/AABEIAKgBKwMBIgACEQEDEQH/xAAcAAABBQEBAQAAAAAAAAAAAAAFAQIDBAYABwj/xAA/EAACAQIEAwYEBAUDAwQDAAABAhEAAwQSITEFQVEGImFxgZETocHwMkKx0RRSYuHxFSOSB3KCFlOiwhckg//EABkBAAMBAQEAAAAAAAAAAAAAAAABAgMEBf/EACMRAAICAgMAAgMBAQAAAAAAAAABAhESIQMxQSJRBBMycWH/2gAMAwEAAhEDEQA/AMhg7TkF1iE31rXdn+2QTDthrigK2aHHIMdZHnNATgEbDXMQr5T8TKLf9JPMeFTXsGLllHQS50IHIDX6/OspNS7KSa6KXE8GyGQ2a2T3TOmu2nKqgxjhSgY5Z2nSji8HuOAEBCtqUJ/CQPlqfnWev2yjsp3BqotPQmqEUb1uuw/BkfIWiWafGF+/nWLw2GzhiN1gx4VsOB3Dgbtu45zWyBtuM0a+OhFTN6ocVuz2JLYAgDSsP2o4ArDMugzTpyrZWMajqGVgQRMzQ65bFxigaQQZrObWsSoruzznDcSuW3NtGG8e+lel2MMFsQe8csknmapYTslYS58TLmbx2nrR/wCGIjlEVSg34Dl9HmHbLDl0UKnfcx7kUJHYq+HRAJz7nkoEE16XxrDIqq5E5f3FWsLj7bZYI2/aojJx+JTSayo8j7S9k3w2XXMCunmDqKLdkex2dWuXwylYKAeu9bHtDaW/ctqO9lMkD0o8IVNF5bRrVuTdqyaqmY/g3B0w11nLAIw0zRvE1ct4SziWXKAxW7nducAbT4mKA9pHxF4PbRCoTXXeNdusipuweAuWnVu9kuIZB5ERB8N6zr1sp/8ADQY3gFpEuMghnYsY8foK8su41sjWGAgEksNzrP616B21t4q1az27ncDEt/MMxgAeFeW3LkBp/Edfqa1gtkN6KjCJp73iwAPKm3bDKsnY/Wom+tdBmXMPc2HMGRR+92ku/D+EViZkj+Ux+1Zq02VlMT4VvGKNhUlBnyGI1LMTsANfOfCs51atFxT8M1wrhv8AEfFdiQEXlG/KgZEe9HMOL9pHQI6fEEGR7+XOg16yU0YEemnvVxu2S+iNDofKnNeMRJjpypiqToBqdNKs4bh73LqWVEOxgA7bTr4aU7QqInxDMqqSSqzA5CdTTcNfKNmXcD9adisO1t2RvxLoY118+dVxvTVUJk++razJPWaVMQQuU6jaI1Gs6HluabhcRkObcwR78xTReIYncnrQIjJ1pJ3rjTZ3oAQnSkJrmGgrqBHBtelJNX8ThVS0jGc7ydtBqR9PnUuP4cqWLNwHvXCZ8I/yKWSGosFs5+VNzr0+dWMDhGuuEUEkmDAJgdTHKt3/APjAQM12SQCe6OYnrUvkS7GotmUt2EIKhyWkwORNPwGKfDtnIlTKxPlPlrQ2zcKtI3Bn9DRri/GUvoqqgVs2dyNpiNPDnUNMtM0tjHK+Ga8r5XAMrz05e1YK/eLuzHc60wOddas8OwD37i20EsfkBuTSSUdjbciFLrDQGPKpjj3KhC0gbfKn8Y4Y+GuZH3iaog61Wnsna0brDdoHuWlS2AjruZ5Qau8I7arhyc6FusHU9dTWM4C3+6qzGbSa2nAOy6vie/DW1112Om3vWUlGLNE20bzs32gGKUkoUYHY8wdiDR6gVvG4dLq2lKqw2A0Hl50cFVCTfZLVFPiOFFwBSYEz7Vle0OEFlkKGMwM+kVs7i6eW1eR9pcU9/EOmZu4SqgTvpmiNwINROKyLi3RrezOIR7gB1aCR8prYmK8N7NcWbDYlM5IUSBmmBJHyrW9ve0qG0gsXe+HDyh1EA8+W9OOtClvZuntKLgJA7wj2FKXtoGY5VVNzsAIrxHE9scU8BrpkEd4AAx6V2J7UXbts2HYQTq+snTnrTpp6RNr7PUu1fE7TYd0VldnHdUEHxk9BXjFxe4xbRt/em4PG/DY/mGsid/7VFiMTmTaOfyq0nYWqNZwjs8cTbgsQioGB/qJ1HpWTx+GNu46HUqxFG+Fdp3s23AJLMduQqrwnBNisR3j3SS7novP9hQm0230N06SL/Z7ggdPjXdLY2HN/Xku+v+at4/tEyDLZAVRoMogR7Sa0OA4e2JQv+C1mKoOWVDkWF56KN9POqmL7Mp/OxPkI+QrOU16WoutGXwnGrrPJPKTv9TVq8q4lWBAV9xGzeHQHxol/oqIDzNZ7EvkbQ6A/f7VfHyqWjOUHHbG9nGtpiFN3RQefI+NXu0HHQl9zh4gqFLRzE7UC4i2Yh/5t/PnVRbegY7SR4z5dKtxTdslSaVCs5YliSSeZpgNcDTZ0960ELyrn5UrNtTDvQSPRCToJ8qmxGFVbauGkmZHjJiPSJp+AxZQPlAJbu6jw38KqorMNATsOuppbGkJdcmJ5aekaVGokx6UjGpUdYMjWgKCPwWupGZQLSyJOhltfPf5VRxWPe5lDmQogActv2FVsx0E0qncR0pJUDNp2C47YwyYlnUtcK92BqQAdB61s7/a/CocpvjQDn1AP1ryrgiq122IMkgT5nf0mfSjWM7I99slxis6aj1+c1zckU3tmsG60ZlNzUl2y6HvKyyJEiJp/DbqpdRnEqGUnyB1rZ/8AUDidi7bti2VLTy5LH71rJtNImKTTZg+da7sHeRLhuGMygj0MftWQO9WsBdKuBMA6GOY6U5K1QRdMLdrMecTe+JsPwKPAc6CYe2XdEG7MB6nSi/GsjZBbQjqT+lB7dwo6sN1M+opL+aQPs2y8LTA3kzmc4gHx5x70S43jWwSo9sn/AHGYQT65v196xuO44964rvplAA5x4imcb4s+IZM+yCB686zwbas0ySTSLnEcc7FHOYd/MTsQR+ler8K7VWiyWmbUrox205E9a8rvWb1y1byqXeYgKZIjnyp4wt4IqZWR8xDZh4cv3o9VC/09mxvG7KIzZ0MaQGEzyFQ4PhVpglwqCwlgf+/U+deG2kcsV3Kzz5r8v8Vqbvb3EWrYtFUmIzA67cxQ03K+w6WiP/qXxSxcvLbtL3rcq7AAAnTQdYNZvAZGzh31I0G2onTxoZeulmLHUkk+szTQ5medaKOqIvZNiUhjpHgau8KVCcjW85Y5s0xCASY6c6r2MM911HNzlBPlWpXsjcXDG/YfM6MwcAgAIN46nqKJSSVDSfZj7rgsSNpMeVOw+HZ4CgknYDc0bs9lrpyOw7jqWzcp109etXOxx/hsWodC41WQJgxoR1ozVaDF+hLiXYJreC+KdLywWWdNSAQT11qHgNj4OGd4hrk/8V/v+lejcSvjFJcspIyxmJ8DOUecV57xq9lX4a6BQF95rJyvVmkI+s0uB4h/D4CwoKBigJzeOvLzqjhuMtdYqyCd5VpU/UUZx/CUbDojqp7oGonly6VRwHBEsW2uLA0hY61nJ2axVArH8TtqcpzFuiqT7xWN4ldDO8SOeog6+B9KO3+FM4zI7LrJI/vWaxaOrnOcxGk9RVcSj2iOS6I7glPI/T+1VVOh8vryogB3G++tCW3rqjs52OBpEfQ6A6c/GmzTZ0qhEhNdz9K60hYwI9av27qKjh1lz3R/TA38qTdBRUw4BOpjf60Y4ZdW08FGOYjLpyG+nWg9uIkj7+zT/iOArd6FmG8fs0mrBE/F7aB1KBgrLMNuDJB+Yoc5EQPGrtlDcc5zEDfkOftrNOvW/iFLdtAXMCRu5Pny/vSTrsOwfzpWRl/EpE7SN9qJ47hL2HVbixBAMxz1+ho1xkWL6PcU/gQqq7DMSfpFJz2h4gjAYhEsrlb/AHC+bYaKAQdd5NMu9oLoJCsCvIkakeNCCaTIelPBPbFkyyN/T61x3Fcd65uX3yoAUJOtK1z8I6Vt+GYOxcww2II16qRoaxmPsBHgbVClk6LaolOM7oFVS8tJ61MmFzCZiTpS4XBs75Bv4+dVpC7K7nWuLGpMZZKOUbcGK5WkBY1nemI9p/6ZYELg1cmS5J8tdKu9qeyiYoh87qygwFMBjGk+sV572V4ticPAVptk6rpAA3M7zWvx/bpEslrZV7nQmAD4jfrWDauqNKfZhuHqip8O4uV1dpLfzLIiRuPChfEL6glmQMGBVfCDv+lVXe7ednys5LFzAJkmSxgetQYi25Izgju5gI/KelVFb7E3aKzsCaU2yNxpFIkgny/akVjqAd961INRe4jZ/hjaCFnyoQToUYb6jzov2S7SsqpZbW0HOZQpLFCNST/LO9YW0S0INyfXoBW17HcHxNnEqHQqjHKzdRvI61jNRSLi7ZvO01oXcMxsnMVU5Qh0nXkOdZPsrw6+t0PctkoUkEakN+9eiYWxbtBbSKe9mPXY6knzNS4y6tm1mI2A06mslbTKy6oCcQRMOruhOa+AIPKPxN7H9K844pdlp/r+Q0rS9peL57sDkPYD9zWI4pdIA15mnFWzTpbPReJcQJKLmChoEk/IUP4pj7ipktujADTbTrJ/MaqdkuJLftgkguuhB5EfQ1FxbDF2JyIB1ETSem7NItNCWsZkQhjrWPu4nPeduUx8o+lWuK49LSlFAk+80Iw1yFB5kzV8ca2Y8kr0EVAyMfD7/Sg93eiWGbumh10amt4mMiMnSkNIdqUjUVoScK6aczEkk7wKZyPrSsCw6N3UEFtIymZLRAnryq3xGw1siznLuCQ6ZSApkQssASZnw0FU8JiDbdHWJUhhI5jUVrMNikvXb2IZWYtkZwATBMgnQfhBG4qJScdjirMsqMoLFe6DB94PzEV6D2LS1icSLsBRaCoqn/tfX3j2obgMFaxNu93SiW85DHcsWJWSddjWSYslxhaZoE6qTqB1jlWbeaa6K/k1Xb4tdxN4JqluJnSGAg5eog1m8Lh1KgF8pbfynnUV/FXDLPPeEAmQCogQPbeqTvPhVxjSolu3Ycx9y29i1btW++ujFRqzEn1MgCiPDuMYa1aRLmCZnA7zEDUnWdfOh/Y3iCWcVbe5qoMbbTt8zWuv4zChmDXLcyZkba7em3pWcpYuqKSvZgHtwR98qbdSI86v3k2PiP2ovwPgZxN5Fg5ZBbTkCKT5aVslKzPJddPwMR5VouEdknxNlr2eDmgA84iZrW8f7A21ttcRiCoDEctDr8qI8T//AFMGTZAIADR4Hcj01rmn+V0oqmaRj6zzC7wS+iNcYZVUxPiDGlXeF8JFxBczkPoZnxitE3H7dy18FwWmAdNIJklqOdp+CILBxFgZWRZKoAA66TI6ik/yJaT1seKMd2z4Ali0HzZnJEnqTyjy/SseVI18q2vaLh7vh1xFy45zLKJvBEgk+GlZ27w1zaa5lhVGpPUETW3FyfHb9Ikt6LGExj2IXMG+IASOk7a8jRzCLg7QdbwUloI8NsyzvNZr/TXa2rgSGbKI3zAjSKTGYd0GRwVIYb+Y9aHUvRqTQc4P2tGEYqlhXScqkmGyyTqY6UG7QcYbEYhrsBQwyqoGiqBoB+tX8T/DNbVmBzg8u7mkHcdBprQFtxpThjd0JtldyS2v3tSokmN9KnxNjI+WZMa+BIGnpXWXytI3j962y1okZg3COGK5gpkrtPhNekdke3NsAricwIPcaMwjaCeUda85dpdiBA6egp+DU5gFBJLQANzrEComlJbGm0fReFxlt1zqykRvI2+lZztfxJQN9EBJ8T4eX6msNwPACzq5zPEkT3E8J/O3yHjQXtNx/wCISqk5ev8AMeXoKSUnrw1jFL5Mmw+Kzu7k/wBugofxN5jyPzNRcHvEq0+P/wBaTFCW9x761WNMeVxKnDHdLhKMVPUGiuK4heiM5oZYEPNE3TMu2ppy2KOkZ3Eks+pmpweXhU9/BlO8aqgwZq0yOi9hny5fvwpuMSCTUqKrRrB8dj59DT8Rh31JU5QBm20BMDXYT1ouiWCyNB6VJk1FEOL30d1KW1RFhIt/hJUatJ3J38avdmeFC+zu34EGvgSRGlTKdK2JK3QAIEmKYNqt8QCLccL+ENp5VGl4Jly6mBuNiDP7VadoREV70UR4dxZ7LBkjuyD/AFA8mGxA5UOa5LZjvv70wtoTSatUxJl7/UboR1DkLcOZx/MZk6+dX+z2EZizgiFAJ8v20oGzmAs6Dl0p9m+y5srESIPiKUo6pDT3s13aa0l1EyOkIHO+gDP3VUDc61kM/LTUEH5fOkV2gamPs05EnMZ2A+ZojHFVYN2yXAcMu3ywtIzkCTA9BryP7VVKdd/Hf1rcdhOM28IjliC90oB/TkZiZHIQZmspinRnc5TqxO/jU/slk6Qy+fqP1Fex9icKiYdSNzqT868n4Nwi5ibi20Gp8RoBufLavS+E8BxdqUDKE8W09O6TXLywk2sfC41Wwt2sxP8AstbU9+4MoA3j8x9qocF4W7YfI7d06ajXLG01fw3DLovteZLbFlCj/cY5QN4Hw+dWcW2ISfh27JUbZrjKfYWyPnWT4py+Ui1JLSAPEeBYawSwEZkIIJmY56+dXWS5/DN3hBQkaco5mvP+L4rE4i6Wa5ZQwVC/EJgTqPwioeI9pcULRtI9vIRl0mRyIknwpfolKtjbpbIeJ8ZvrbCZkKEEaDUA768jv70NxXF3+CcMCMkzMd484J6TVBMM7kLIJY7Cf0G9Wf8ASXFwW7jBC0bqx0PMAanyrrXHGNJ/6Z3Jk3DuJfDQq2oBzL4MCI8vOo8fxC5fLBu9L5v6t/77UW4xw61hmQm8jyCIS02aOrK9wa+tVsXhEQLiPikgsGARQr9RGrBTPnU/Fu0gcWaHgnZsYrIj28gthc7SQTM90iPxc5rPdquErhnyLmgMVM+Ugg85EGjC/wDUFlBCC6Cde8bME9WiyCfes/x/jBxZRnz5lJGgXnzlVE+VKHHNSTfQOmgSqMziASTPmaaxgnyojjLS23RcO9x2ymTA0nUhQFnar1zhOF/h1uLddrxbKbc6jfTKBO8a1s2lQsWZ0v3vvpWj7O2giPfcxvBPJdiR4nb5c9O4V2b+KjuEYZAT3mYDQazqCfSqXaJyLSogIRTB3iQNFJ8JnXmfCmmpOkUo0smN43xzOuRDlT83VugPh5f2rOO5OtNLUhOtdCilohybCnBzq4/o/QiaJFc0nquYe1DeBCbmu2Uj33ozYt5XVZ/KfUSY+RrKfZpDoo21Gb1oub9u0mZyB0HM+AHOsxxC6yucrEeVDmckySSep3oUL2JyoJ4riTXnP5UH4V6eJ6mmESNOsVTw34qursx6GnJV0SnY7Py9vOrmFx0p8NmhSYn+XX5ihuIbveVc5kT9zTXRLLvEbBV2UEMAYkbHlIqfDcYvWrbWkGUMZZgveboCfCmYcFwGmY3ExMcp8ausM4Cdy2syN8wnkXO4FRKumhpfQEFt2nQkk0+7g3UgMhXz/tWhvdnW0ZL1lwel1VI10zB8vyrl7L4hhIyNHS9bM+Xe1oziGLM4MO3MEelL8AxEGfL9zR9ey+J3KED+aZX3WRVS7w5kMMV9GDfpNCkn0LFgk2H/AJT8v3pRhn6fMUcTAW8gOYsxYLAGw56yNa65gAp/DlPNXdB7agjqDrRmroeDBmEwdxyEGUSZ7zAa7aGpMRgLlh2V1ggag6nby31qzZwJcZhAXbXr5xFc+FYad3UaM5IHLYnp1oy2GOijYQlYZBlmQytBBj1n1pjYd/yh2HWd+vzmjdrgl14yp13dNwOrEedd/wCmMT/7XsQR6EHWllH7F+ts0fYfGWsNeZ7pOq5RCzuQZ7vl0516hhuJpcUMmoPio9wTNCeFcAwRVSEV2gSSxb5THyogvZ7DBswtIPIbeNQlKrXpeumC8f2lFm7lYPLAaAKVH/lIqfG9orGUAtmBGqp3j65Z0q5/o9tSWCSPF2geAXpQHivZm7ccNZFu2oGhUura9dx6issJdWy7j3QP4lgcLjRFi3kfmYCf/HMNfSs/juyf8OA95S6T+WfYkHu0Wu9jsVmPcRp/NmHvJ1n0oa/xbZZC6nL3YMMNPcA68jVYSTpMLj6O4VhbRuJ/DWYcGR8RxE6wROprQ3ez9y46PfVldfwsnwiAPHMZNUOy/Ebdl2Z1En8wSSPAQRlHpWsv8XwzoWa4CpH4Syg+gJmaT42nYZrwC8S7HteUA3FWPzOqyR45In1rOf8ApE5mVMRZbKdZMD2g0QvNg1cMtq4681ZoHy1PvRjB9oLCnu22spzKgGT5DlTcZJfEWSfZgcfhMrMrgl/5lgqfLQCPKq6YMxmKPHUafOK9MxvaixlCqXcHeVGn/NYNZnHcXvfhS8xQ/lyBYH8u0H0pxy6oHXbM3hsK5YCXEn8mreQ50TPA1zDNeVC25vIykepBU+9VUzSO8y+MHT22qS6HYjvs3nv6ztVOOxro7HWmw6lVa26kaG3qCeWvLWKL4DhqvhQjAMHzFtN5ME+en6UDuYcmFbWTtz1kDUeMfKvRFwIs2UTcqoBJ3JjU+9TJYouLTPAMbhWt3HtkHMrR59D6jWrbYREXv95jynQe29antT8P45fKMyrBPUnUA+Q/WsjjLuYzXbB3FNnNPUqRPw7GIj6ggQR1ij1lMy23Gogz5a/sR7VjjR/sziu7cQ/lWV/8u630+dZ8i1ZcH4DOK6uTVAVfx7DP4VSdYpw6Jl2SYfVhV+1+YeNUMN+MVftDX0pSGiu51b7500HQx1+/0ojgODXbrEKInm2mgmidzsytlGd3LEKTAEDQT51LnFasFFsF8EcG4E/n7o/7j+H3OnrWhvcNKNDZ1PQgj3DCaF9neFgML1zMCpDIojRhqGafkPWtLicVcuauzMB/MT+9U4tsjKkC0wuUyG+/Wo2ww5/Lar7AdKgKnWI9v70OAlP7G2LRUEF7igiIU8j1BIkfvT7eFw4JDPcHSEUn1Gemi2f6fLakZCdtPr/apfGy/wBhbDIqEI9yZkEOyCPFQIHoedNS6xBD3rmvXvqY6hj08KhVCd4P19aeXOxn7/WocKKU7LuHwtgrmOIRW2h7DQJ6FJ38Qaifg9vcX7LHaO+vyZAKggabHnppHjFJbtazmPSPrMRFCVDbsVLCISGeFO4suQInXQk6xpzq3HDhoXxX/wAK7D32TVXH/wDRQ36g0p4iTvatnxyLr8qHBsWRcTFXFjI7L/2yJ9qJ8O7QXU0ZnJ6tLe8/ShqsAasHFkiCqkeUVtLiTM1yUXcR2ovEiLuXWMuUEeEwJFE8TxDFqshuW65GnTpyrKvlJ0EUqEg6Eg1D4ZXdl/uj1QWu9p8YBB0n82TX9ImgyMxYsy5iSTqOZ3qYuWPfYx6n5VNZuW10aWn0IrRQozcrZascUCDuoqt1gN8jVG7jHZj3yAT6ewEVJfW1urkDpH1qrcZToDp1I1+VNRj9A2yw+OuKCguSo6RH6UPyetGsFgMLcXXEFH5hlhfT/NEBwu2ihVOHuk6y0jXpIapcox8GotmYVZ5VEU8fcj5eNHMVwK5mLi0jKdkt3CQPTeieA4dh2XK9oK0flu5j4yrQR86T5EGDMnhcI91siDM28SB+pFWP9OZXyPFs7y8x4fhB0o/e4JhQ2uJCD+WNfc1Yt8P4eohr5fpLHTyCijK+hVXYF4TwwfxNsZkcZs0o0/gk6zqNqOdp8fkR2kSBpPXlVrh+BwqEvh2zELB70wD+m1eedtuL53KA6Lv4ms6zkkbJ4xsyXE8TmJ1J8TuSdyaEM1S4m5JqtXSzFCE1c4RdyuTyKkH5VRY0+yxG1TJWil2T4xpNQXGnXqKVmnSoidKUQZYwSy09AaLYK3mYLzP6UJwrx61peyeHNxy3TT6n6VHI6VlwRpeCWYMHcL+hIqxjUkxp67etKi/DvAcmED1gj5hvertqwj3UR2yqxOYzEQDGvnXFfzR0tfFgW6kaRHkZqEvGho3xLhmRgqEFCdGDgwepOhFVcTwG+ozBGdYnMJPuRNehGSdHBKLQN9qaDB/epnwhI/EJG41B94piWeu/htVWSNdSeUH2FM+GR196sMqgTn9B+L0kEVFnnSffl8taBiXbDLBJiRI2JjynT1pCs9fOP3pQ0nlT0FCER/CmIkEc9NZ9NK4of5pqcL9mm5Y00p4oE6IGQ67e9Ot2xA1PzqYrodKq/D8T6z+9Q4PwpSXocCjmdeW2o8/OKazA6TUKdAPff+/KnqNz+o/Wa2IHZT/mudWj8PzBjz5/4rnkbR7bVyEie99KVAmKlpjqSBz3pjJ1Me1P1+eum9LnbaT98qKYWVkWfwk/8TH6a1IEPU/WngmuY6/5pUOyIoTpBjqdqeLdKxNMJOm/uefhMUUFiskff0pGAHKml43I1085pc8+A60tDEJ5VxgESYpEfmBHpXQPemI1Nl1w2Ae7rLkkTufyp+/rXkPE8TmJJOpr0Lt5xELh8NZU6ZFdv+ML/wDY15bibkmogqTl9msn0iB2pk1xprUxCMaTMRSVxNAHTSl5pKQigY9Wr0TsBbAtFuZY+1ecg16V2HtlbCyCJJOviTWHO6iace5B3idoET038vDxBg+lQYLht3EMyqySoB1MFp2gczpVi/c0NBkuwToZnQ9K5uFZSNuV1EvYvg121+O2wHUiR7jSpOH4a7mARwk7S+UH51Xfit8CBdYjaC5jy1mao3LrGZAb2AGvXl7V3bqmcaSs9EtYdyn+5hrL/wBSiZ84k1lePYW2CCiBI0YB80f+BAYGoOCY1LU5nu2jvKGVbzB0J8as8S4h8cH/AHkYD/3AEcH9D6dKx+UZGlJoAOgP5gI2ENqPaPSmiD9aedehHX09v15U1kEjT56+lau2ZpUxygaxPtTH01n761wSff7On3pU7lNsrTH4pkE+omPLpUpyTKaVEWZo0E+Owpye9IgO2utIDFbRdmTX0TsFiII8Y389ai0+5pBJ233pmY9KoQYKT3gQD0g/T60mWd5+9jU7plMSD4Dl6j70pYHgfL/FUIhKRyj6VxA+xUjHmANPXXxn1prSddKAGqI1H0mnFB586b9/c07lMEUANHl9/f0pG57e9NXeDt9Ka+raCNevXqdjSYCufud6ijp+kx6/e9PNttdNfHT9BSOY5kDrB8qhsqiItGgk/sK4LprtPPpy086512lt99PXWdOhrmaeftH0pWyqQuTTek9Ps/2qPPykAefOef30qlcxTIGkzzBH6HT7immKgR2gxpdzJmBlHkNBWbdqu4+7JoeTTf0Ujiaa5qXD2Wd1RFLOxCqo3JOwFHh2KxOXM6hJ0Ck5m9QsgDxn0rOUoxVtlKLfRmqStinYgKe/iRPNUQyPViP0olh+y2FXcPcPV2gf8Vge9Zvmj4ylxyZ58lssQFBJ6DU+wrRcL7JXbkM5yL03b9h862+GsWbYhERf+1QKlfFKOdYz55P+Uax4kuylw/s9ZswQgJ/mbU+529KJaDaqT47pVPEcQA/E0VztTk9micV0X8ZiYFU1ciKrpfzx3TAP4iNx0A86kRBEa++ntNdn4/Himzn5ppukPY+dKG5j799aYbYkdRtrSz6ffWuq2c7Q/OfSmMoOpjTw+sVwQ/f3pXQf8UCQhXqKVVjlXEaAnXwiSD+29cF57UgJDG0R16U1xHOfl86XU84pMvrQ1Y0yLOswZ8zt66U5CDz+9KUgUuT7/vSiqKbsjeOUTTY8af5a6xodqSfD9Kdk0GdplQPSlUSeYHl68q6uq0Jky4eRKwR4kT7AnwqM4VomQY0PIz4A/Surqm3ZVELqVkb8jGxMciDrUe/y2/eurqZKGDc6z79PDf8AvTmPPcb/AOB97UldUJlMa4kDX5nTXpSNprt15COuuw8a6upiEcgyD009NT5aR97RoszExy6enKda6uqWUhLhWQdzECD894oVx+6VQA6Ty8tBXV1Eexvoxt9pNQ11dTYzR9mMMyH42zRCeAOhPrt/mtJZ4ubTpcaXiREn8wIHzikrq4eT5N2dC1HRSxfGWd2dohjEgQAQNAOugqm/H1Ub0tdWkOONEZOik/aJuQJ/Sq1zjVw7NHkPqa6urePHElyZXbHO27v71E4LbknzNdXVeKItmwwFybSSdcg5zqAARtptzq0Lh5j5EfSurqESIL0felPS8Ovt+1dXUyWOzjpTcwPOK6upgOmesculShzsBPnXV1SNCsvkD4UwOscvPp5+1dXVJReweOtLpct518IB85ifnT8Z/DMs2/iIeh1U++orq6plpleA9CuzSs8wNfUaTS/CX+f3U11dQ2Sf/9k="),
        //     Resume = "og det var det "
        // };
        
        var api = new Client(BASEURI, new HttpClient());
        var response = await api.MovieAsync(movieId);
        List<Actor> actors = new List<Actor>();
        try
        {
            foreach (var actor in response.MovieDetailsDto.Actors)
            {
                actors.Add(new Actor{ID = actor.Id, Name = actor.Name, BirthYear = actor.BirthYear});
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        List<Director> directors = new List<Director>();
        try
        {
            foreach (var director in response.MovieDetailsDto.Directors)
            {
                directors.Add(new Director{ID = director.Id, Name = director.Name, BirthYear = director.BirthYear});
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        Movie movie = new Movie
        {
            Id = response.MovieDetailsDto.Id,
            Title = response.MovieDetailsDto.Title,
            ReleaseYear = response.MovieDetailsDto.ReleaseYear,
            PosterUrl = response.MovieDetailsDto.PathToPoster,
            Rating = new Rating{AverageRating = response.MovieDetailsDto.Ratings.AverageRating, RatingCount = response.MovieDetailsDto.Ratings.NumberOfVotes},
            Actors = actors,
            Directors = directors,
        };

        return movie;
    }
    
    
    
    
    
    
}