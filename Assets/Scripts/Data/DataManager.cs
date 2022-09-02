﻿using System;
using System.Collections.Generic;
using System.Linq;
using Net;


    public class DataManager: IDataManager, IReadReceiver, IPhenotypeReceiver
    {
        private List<ReadData> reads;
        private List<PhenotypeData> phenotypeDatas;

        private List<PhenotypeData> eyeColors = new List<PhenotypeData>(){new PhenotypeData(Phenotype.Eye, "brown", 1f), new PhenotypeData(Phenotype.Eye, "blue", 1f), new PhenotypeData(Phenotype.Eye, "green", 1f)};
        private List<PhenotypeData> skinColors = new List<PhenotypeData>(){new PhenotypeData(Phenotype.Skin, "white", 1f), new PhenotypeData(Phenotype.Skin, "brown", 1f), new PhenotypeData(Phenotype.Skin, "yellow", 1f)};
        private List<PhenotypeData> hairColors = new List<PhenotypeData>(){new PhenotypeData(Phenotype.Hair, "brown", 1f), new PhenotypeData(Phenotype.Hair, "blond", 1f), new PhenotypeData(Phenotype.Hair, "black", 1f), new PhenotypeData(Phenotype.Hair, "red", 1f)};

        public DataManager()
        {
            reads = new List<ReadData>();
            phenotypeDatas = new List<PhenotypeData>();
        }
        

        private List<PhenotypeData> GetRandomPhenotypes()
        {
            Random rnd = new Random();
            PhenotypeData data1 = eyeColors[rnd.Next(0, eyeColors.Count)];
            PhenotypeData data2 = skinColors[rnd.Next(0, skinColors.Count)];
            PhenotypeData data3 = hairColors[rnd.Next(0, hairColors.Count)];
            List<PhenotypeData> dataList = new List<PhenotypeData>();
            dataList.Add(data1);
            dataList.Add(data2);
            dataList.Add(data3);
            return dataList;
        }

        public void LoadDataFromCSV()
        {
            
        }
        
        public ReadData GetRead()
        {
            //if (reads.Count <= 0) return new ReadData();
            //return reads.First();
            ReadData read = new ReadData();
            read.data =  "CATTGTACTTCGTTCAATTTTTCGAATTTGAGTGTTTAACCGTTTTCGCATTTATCGTGAAACGCTTTCGCGTTTTTCGTGCACCGCTTCAATATACCAAATGTCATATCTATAATCTGGTTTTGTTTTTTTGAATAATAAATATTTTCATTCTTGCGGTTTGGAGGAATTGATTCAAATTCAAGCAGAAATAATTCCAGGAGTCCAAAATATGTATCAATGCAGCATTTGAGCAAGTGCGATAAATCTTTAAGTGCTTCTTTCCCATGGTTTTAGTCATAAAACTCTCCATTTTGATAGGTTGCATGCTAGATGCTGAAGTATATTTTTGAAAATTTGTCGATGCTACTTAACTGTCAATATGGCCACAAGTTGTTTGATCTTTGCAATGATTTATATCAGAAACCATATAGTAAATTAGTTACACAGGAAATTTTTATATGTCCTTATTATCATTCATTATGTATTAAAATTAGAGTTGTGGCTTGGCTCTGCTAACACGTTGCTCATAGGAGATATGGTAGAGCCGCAGACACGTCGTATGCAGGAACGTGCTGCGGCTGGCTGGTGAACTTCCGATAGTGCGGGTGTTAGACGTTGATTCTTATACCGATTTTACATATTTTTTGCATGAGAA";
            read.quality = "$&'(-(((43,579'((,'%%'(&%$$49]CA>:3211:=CBA26--/5;99')()-77>@@?DC@6262356666636560-+,79,,,*)*4@A943465D@BB<;;;90,158:=@:/..9988-*,()--9<;<10-/))*)+)&.)'&&%%.00//2.//59767.++.2.-*(''')+61***./,,@]]]];:::11.+,7::CAE?<D]<889?4@?92]/02..00.&&%%'37:0,,+./'&)'436@59ff02df-872f-48a6-9617-abaf4c52385e runid=51bf4723185f4e8abde277e8f9e7dfca497713b0 read=16 ch=391 start_time=2022-06-23T10:22:44.714574+02:00 flow_cell_id=FAT29260 protocol_group_id=Rapid_Lambda_control sample_id=SK1420024 parent_read_id=59ff02df-872f-48a6-9617-abaf4c52385e basecall_model_version_id=2021-05-17_dna_r9.4.1_minion_96_29d8704b";
            read.signals = new[]
            {
                564, 536, 477, 531, 491, 555, 567, 576, 464, 523, 569, 526, 456, 389, 404, 421,
                417, 410, 404, 431, 379, 386, 403, 410, 399, 416, 388, 364, 364, 373, 378, 408,
                414, 418, 437, 395, 404, 391, 409, 392, 400, 413, 429, 423, 418, 426, 404, 405,
                407, 412, 425, 415, 390, 366, 392, 371, 358, 380, 366, 343, 340, 328, 324, 328,
                417, 439, 446, 491, 541, 514, 532, 532, 525, 514, 521, 480, 438, 442, 441, 505,
                438, 433, 359, 421, 433, 412, 396, 387, 366, 370, 442, 489, 502, 489, 492, 398,
                407, 414, 381, 386, 399, 387, 420, 416, 422, 455, 450, 435, 455, 443, 447, 459,
                440, 443, 434, 440, 437, 438, 458, 441, 433, 491, 491, 487, 496, 489, 499, 473,
                500, 494, 491, 495, 482, 496, 504, 503, 486, 512, 514, 487, 506, 492, 486, 492,
                492, 489, 503, 553, 586, 572, 534, 543, 570, 555, 561, 547, 579, 544, 480, 459,
                452, 431, 429, 465, 472, 531, 523, 519, 500, 484, 495, 467, 477, 458, 469, 489,
                484, 445, 546, 480, 494, 463, 494, 475, 497, 543, 553, 547, 531, 530, 556, 551,
                514, 530, 542, 508, 470, 502, 531, 536, 514, 529, 530, 501, 463, 478, 509, 509,
                484, 479, 485, 497, 476, 473, 486, 481, 466, 499, 466, 452, 465, 483, 507, 506,
                516, 538, 511, 494, 526, 510, 502, 510, 541, 503, 517, 533, 517, 509, 511, 514,
                508, 502, 505, 511, 512, 504, 512, 495, 498, 498, 498, 496, 495, 491, 509, 482,
                481, 487, 496, 508, 530, 509, 532, 505, 510, 518, 521, 524, 527, 518, 518, 474,
                561, 574, 535, 619, 449, 583, 583, 578, 574, 483, 534, 541, 577, 577, 508, 413,
                515, 457, 488, 441, 502, 452, 409, 464, 470, 495, 501, 496, 473, 471, 482, 490,
                456, 495, 506, 486, 492, 470, 476, 476, 445, 469, 497, 463, 502, 546, 552, 567,
                509, 519, 467, 522, 493, 452, 410, 447, 511, 453, 479, 479, 481, 529, 555, 530,
                696, 454, 504, 455, 526, 516, 486, 463, 463, 507, 493, 318, 762, 491, 551, 505,
                489, 475, 484, 500, 501, 478, 431, 509, 491, 506, 507, 492, 480, 447, 426, 460,
                441, 402, 434, 418, 396, 381, 670, 403, 428, 379, 392, 388, 468, 495, 488, 470,
                453, 460, 449, 460, 442, 441, 477, 456, 506, 742, 746, 748, 752, 708, 561, 494,
                481, 509, 490, 481, 482, 493, 456, 512, 491, 493, 492, 476, 463, 472, 471, 457,
                458, 453, 450, 474, 469, 451, 469, 443, 460, 461, 448, 425, 427, 449, 460, 456,
                467, 477, 470, 505, 514, 517, 526, 525, 526, 524, 506, 533, 512, 495, 492, 512,
                501, 495, 495, 494, 502, 375, 403, 405, 392, 381, 385, 374, 410, 403, 398, 423,
                368, 421, 372, 345, 349, 407, 374, 341, 348, 375, 366, 350, 434, 454, 397, 366,
                411, 352, 366, 380, 371, 364, 363, 355, 356, 347, 362, 354, 351, 317, 335, 360,
                368, 386, 382, 396, 397, 331, 314, 343, 342, 337, 359, 342, 377, 371, 361, 342,
                337, 343, 338, 331, 326, 330, 317, 350, 363, 364, 351, 345, 359, 381, 360, 341,
                365, 350, 356, 357, 362, 471, 455, 437, 456, 469, 458, 453, 458, 462, 442, 435,
                455, 437, 440, 426, 417, 405, 414, 395, 389, 376, 381, 381, 370, 365, 391, 393,
                394, 363, 475, 497, 500, 511, 508, 466, 505, 477, 504, 480, 484, 466, 492, 448,
                423, 379, 389, 394, 446, 453, 442, 481, 458, 421, 429, 417, 430, 427, 435, 423,
                426, 389, 392, 401, 395, 389, 398, 409, 415, 404, 395, 385, 404, 407, 383, 377,
                385, 391, 379, 363, 376, 385, 381, 468, 487, 497, 493, 465, 472, 476, 506, 490,
                512, 494, 517, 497, 495, 506, 499, 470, 475, 468, 501, 477, 491, 490, 482, 514,
                473, 459, 427, 455, 446, 452, 450, 452, 448, 472, 440, 422, 447, 439, 414, 412,
                423, 423, 462, 448, 473, 488, 494, 441, 425, 398, 394, 402, 412, 396, 454, 476,
                473, 502, 518, 505, 517, 501, 480, 460, 474, 464, 461, 441, 480, 468, 485, 487,
                466, 481, 444, 452, 472, 484, 475, 455, 453, 483, 459, 482, 508, 559, 566, 553,
                565, 553, 549, 561, 560, 527, 560, 543, 555, 515, 505, 528, 506, 475, 418, 437,
                431, 431, 424, 430, 431, 432, 434, 453, 467, 444, 465, 447, 453, 432, 456, 460,
                428, 447, 461, 430, 439, 444, 448, 459, 442, 447, 433, 448, 443, 461, 449, 418,
                453, 465, 462, 458, 506, 511, 509, 529, 522, 555, 514, 514, 521, 511, 521, 509,
                503, 500, 514, 509, 499, 420, 419, 418, 434, 408, 407, 417, 416, 409, 417, 420,
                422, 433, 423, 447, 431, 439, 423, 449, 444, 444, 456, 446, 419, 452, 437, 445,
                429, 444, 440, 457, 449, 424, 454, 457, 490, 500, 513, 500, 465, 455, 454, 446,
                450, 457, 493, 493, 501, 506, 496, 491, 488, 478, 514, 453, 389, 385, 434, 472,
                499, 486, 492, 472, 472, 494, 468, 463, 416, 437, 454, 490, 489, 494, 499, 420,
                475, 412, 349, 313, 442, 389, 392, 362, 351, 350, 347, 373, 420, 416, 442, 412,
                441, 480, 723, 728, 751, 755, 792, 753, 732, 763, 737, 733, 702, 740, 729, 662,
                678, 698, 707, 681, 689, 691, 693, 711, 692, 693, 705, 499, 483, 521, 489, 502,
                505, 476, 511, 547, 499, 537, 474, 627, 748, 758, 744, 742, 737, 637, 556, 526,
                482, 507, 513, 528, 520, 413, 494, 489, 480, 436, 507, 458, 411, 418, 417, 380,
                479, 458, 426, 522, 436, 438, 448, 453, 450, 430, 457, 441, 406, 397, 477, 571,
                549, 385, 345, 370, 359, 373, 386, 354, 379, 374, 366, 377, 403, 415, 455, 429,
                397, 413, 422, 455, 402, 413, 496, 439, 455, 463, 481, 527, 519, 530, 532, 512,
                537, 554, 515, 497, 534, 525, 520, 502, 499, 487, 501, 511, 517, 502, 517, 497,
                515, 526, 518, 460, 385, 414, 403, 427, 436, 396, 449, 438, 455, 442, 454, 464,
                468, 460, 474, 484, 458, 504, 523, 482, 505, 497, 530, 527, 516, 489, 525, 491,
                485, 489, 494, 491, 487, 494, 477, 504, 482, 506, 479, 501, 508, 499, 497, 490,
                492, 488, 466, 505, 496, 490, 499, 477, 495, 484, 483, 489, 383, 395, 385, 371,
                373, 397, 400, 388, 389, 392, 381, 394, 383, 415, 410, 409, 390, 389, 362, 367,
                346, 355, 341, 355, 369, 336, 351, 353, 369, 357, 350, 344, 351, 360, 358, 363,
                351, 342, 372, 356, 374, 359, 348, 343, 356, 353, 370, 358, 350, 353, 341, 353,
                345, 356, 346, 342, 357, 363, 339, 347, 351, 353, 338, 342, 361, 353, 343, 361,
                354, 366, 372, 353, 359, 374, 348, 353, 368, 355, 369, 330, 352, 363, 355, 369,
                356, 340, 368, 364, 342, 353, 352, 343, 361, 365, 345, 380, 360, 343, 365, 365,
                370, 377, 446, 412, 431, 413, 424, 428, 533, 513, 516, 489, 427, 399, 367, 375,
                375, 372, 368, 465, 481, 465, 454, 429, 466, 399, 393, 382, 409, 381, 398, 395,
                391, 390, 399, 361, 369, 359, 377, 350, 364, 395, 433, 459, 417, 429, 505, 515,
                483, 503, 514, 498, 415, 411, 405, 415, 415, 416, 406, 381, 401, 423, 430, 421,
                436, 408, 392, 375, 399, 394, 403, 396, 380, 391, 409, 343, 371, 360, 363, 367,
                392, 386, 383, 404, 396, 399, 419, 453, 490, 538, 545, 538, 565, 539, 545, 534,
                548, 526, 540, 542, 519, 505, 507, 461, 409, 405, 425, 440, 444, 427, 422, 432,
                421, 425, 427, 421, 414, 442, 429, 494, 458, 497, 442, 428, 456, 496, 500, 473,
                494, 447, 478, 473, 481, 449, 490, 470, 470, 502, 466, 483, 458, 533, 525, 508,
                533, 507, 520, 498, 523, 531, 509, 474, 528, 541, 532, 518, 526, 512, 399, 320,
                401, 325, 341, 315, 302, 323, 344, 320, 325, 335, 324, 338, 357, 416, 419, 408,
                407, 388, 411, 391, 386, 384, 429, 425, 502, 532, 524, 531, 448, 438, 437, 458,
                439, 416, 394, 388, 409, 399, 404, 402, 379, 369, 371, 362, 377, 395, 395, 387,
                414, 396, 390, 415, 383, 386, 398, 396, 391, 409, 415, 396, 415, 467, 483, 492,
                466, 475, 464, 461, 460, 471, 456, 429, 454, 450, 464, 443, 459, 467, 442, 463,
                472, 465, 509, 534, 521, 526, 541, 545, 537, 547, 546, 552, 549, 544, 537, 537,
                522, 465, 495, 478, 504, 468, 489, 498, 478, 496, 474, 490, 533, 534, 556, 520,
                528, 535, 527, 499, 428, 436, 406, 400, 418, 435, 436, 420, 411, 425, 416, 427,
                418, 439, 422, 442, 481, 521, 538, 522, 533, 513, 530, 513, 540, 519, 532, 528,
                514, 463, 487, 494, 499, 508, 503, 488, 485, 392, 402, 422, 414, 406, 407, 389,
                408, 400, 405, 393, 403, 386, 390, 401, 423, 391, 459, 487, 495, 464, 475, 487,
                473, 494, 443, 479, 465, 469, 478, 459, 467, 461, 487, 474, 478, 467, 454, 475,
                475, 465, 455, 468, 476, 461, 474, 490, 466, 486, 462, 450, 460, 408, 399, 394,
                401, 414, 409, 423, 424, 417, 399, 402, 417, 395, 416, 412, 424, 428, 429, 422,
                487, 519, 512, 514, 511, 499, 516, 492, 497, 491, 482, 498, 517, 478, 485, 476,
                488, 483, 433, 476, 456, 460, 470, 450, 453, 366, 373, 371, 374, 353, 368, 366,
                376, 411, 394, 386, 366, 384, 380, 404, 363, 409, 383, 382, 386, 391, 369, 387,
                405, 423, 406, 413, 426, 415, 405, 373, 382, 380, 392, 403, 391, 415, 430, 416,
                422, 426, 426, 409, 398, 411, 424, 416, 427, 460, 484, 484, 487, 491, 488, 491,
                490, 479, 484, 504, 481, 457, 465, 484, 462, 457, 470, 380, 411, 393, 453, 481,
                469, 431, 468, 481, 485, 494, 513, 482, 495, 483, 472, 462, 472, 464, 469, 453,
                452, 460, 467, 396, 372, 382, 383, 387, 396, 369, 400, 384, 391, 451, 437, 441,
                450, 443, 420, 423, 409, 431, 437, 477, 472, 475, 476, 441, 446, 507, 468, 452,
                475, 464, 460, 458, 459, 456, 456, 440, 459, 452, 459, 450, 458, 460, 446, 463,
                435, 403, 400, 412, 397, 390, 418, 446, 445, 456, 445, 450, 446, 441, 453, 449,
                447, 480, 512, 485, 482, 476, 472, 489, 475, 475, 491, 487, 507, 504, 498, 466,
                452, 441, 444, 468, 443, 443, 465, 447, 451, 449, 460, 457, 456, 456, 468, 468,
                462, 463, 460, 457, 480, 475, 478, 498, 477, 489, 490, 485, 490, 533, 534, 514,
                530, 508, 523, 509, 503, 498, 507, 498, 510, 441, 436, 431, 427, 420, 427, 436,
                431, 447, 524, 510, 484, 489, 509, 512, 495, 475, 428, 423, 424, 417, 427, 430,
                419, 414, 415, 414, 429, 411, 426, 418, 416, 420, 418, 417, 432, 396, 418, 412,
                417, 411, 411, 423, 393, 418, 416, 417, 419, 444, 426, 463, 472, 523, 502, 527,
                531, 512, 538, 530, 525, 535, 548, 563, 568, 565, 581, 567, 554, 569, 555, 533,
                535, 530, 530, 522, 510, 514, 554, 512, 502, 527, 473, 394, 417, 409, 397, 429,
                426, 424, 387, 399, 432, 430, 452, 455, 453, 463, 444, 454, 441, 435, 466, 488,
                435, 385, 413, 407, 416, 423, 426, 506, 521, 525, 528, 526, 512, 509, 534, 507,
                548, 529, 507, 503, 495, 432, 367, 344, 343, 373, 387, 360, 388, 360, 357, 373,
                371, 344, 385, 384, 376, 361, 357, 383, 375, 368, 399, 462, 475, 465, 469, 466,
                460, 442, 456, 516, 519, 511, 524, 523, 512, 510, 495, 519, 486, 527, 534, 516,
                512, 550, 558, 545, 525, 544, 520, 570, 535, 519, 535, 550, 575, 491, 492, 432,
                403, 392, 447, 428, 440, 446, 440, 457, 497, 439, 444, 416, 441, 459, 443, 458,
                496, 487, 463, 488, 474, 475, 458, 504, 475, 450, 445, 489, 546, 553, 560, 555,
                546, 567, 589, 705, 607, 607, 630, 641, 625, 540, 593, 585, 526, 510, 492, 523,
                514, 554, 557, 604, 622, 597, 585, 595, 552, 584, 598, 602, 575, 627, 582, 571,
                579, 600, 602, 576, 544, 577, 543, 593, 554, 567, 590, 599, 593, 561, 606, 450,
                481, 502, 481, 476, 517, 427, 431, 431, 451, 467, 510, 461, 464, 482, 488, 483,
                496, 483, 486, 448, 567, 720, 737, 740, 477, 489, 482, 454, 461, 479, 502, 485,
                513, 479, 540, 504, 472, 457, 575, 605, 543, 567, 542, 496, 553, 549, 551, 582,
                554, 558, 546, 564, 551, 544, 497, 528, 538, 495, 521, 513, 553, 570, 504, 553,
                563, 500, 519, 581, 556, 550, 537, 566, 578, 551, 571, 552, 544, 563, 545, 570,
                532, 476, 491, 475, 379, 434, 419, 438, 460, 422, 441, 387, 453, 427, 480, 491,
                502, 515, 517, 483, 534, 516, 470, 489, 585, 587, 545, 562, 538, 528, 520, 533,
                528, 568, 478, 478, 493, 503, 506, 482, 461, 445, 469, 475, 452, 433, 432, 445,
                452, 452, 457, 512, 502, 494, 511, 527, 512, 436, 454, 479, 420, 413, 444, 494,
                490, 491, 465, 492, 461, 514, 434, 481, 497, 454, 489, 475, 519, 479, 475, 454,
                449, 461, 481, 489, 473, 461, 461, 455, 456, 465, 473, 456, 506, 482, 478, 394,
                395, 389, 381, 369, 376, 386, 399, 373, 390, 398, 388, 391, 357, 392, 387, 397,
                461, 454, 485, 448, 517, 479, 499, 491, 497, 525, 416, 376, 396, 369, 412, 375,
                375, 399, 417, 397, 401, 384, 450, 469, 461, 463, 485, 467, 489, 476, 482, 460,
                429, 410, 437, 417, 405, 416, 436, 433, 426, 401, 422, 406, 408, 430, 436, 425,
                434, 422, 442, 415, 419, 404, 401, 412, 405, 394, 412, 403, 417, 398, 442, 536,
                531, 517, 521, 515, 528, 529, 518, 504, 523, 510, 513, 513, 520, 519, 557, 536,
                550, 537, 549, 553, 466, 536, 506, 522, 503, 547, 535, 465, 420, 401, 392, 400,
                406, 415, 414, 386, 406, 395, 407, 413, 425, 441, 464, 448, 449, 442, 423, 428,
                437, 434, 444, 438, 451, 506, 498, 483, 476, 486, 504, 486, 494, 490, 483, 471,
                478, 484, 487, 493, 493, 467, 500, 490, 477, 491, 484, 481, 496, 489, 476, 488,
                500, 508, 508, 500, 482, 494, 453, 443, 444, 449, 399, 408, 425, 425, 383, 362,
                408, 438, 416, 455, 374, 381, 413, 427, 412, 508, 398, 414, 409, 420, 444, 400,
                427, 434, 414, 367, 427, 395, 332, 397, 455, 372, 360, 343, 336, 352, 418, 453,
                484, 441, 355, 444, 493, 457, 461, 470, 446, 395, 348, 407, 375, 354, 367, 358,
                371, 351, 391, 390, 363, 549, 665, 648, 649, 638, 581, 344, 349, 372, 350, 376,
                371, 339, 361, 311, 370, 355, 383, 383, 384, 398, 348, 394, 393, 400, 376, 383,
                461, 559, 596, 584, 576, 583, 579, 572, 573, 562, 591, 567, 601, 519, 516, 601,
                581, 596, 566, 588, 561, 544, 596, 451, 561, 543, 559, 571, 611, 589, 543, 570,
                536, 558, 586, 558, 550, 549, 432, 465, 458, 480, 494, 499, 523, 548, 554, 564,
                526, 569, 533, 552, 536, 519, 498, 489, 500, 480, 526, 517, 495, 488, 484, 528,
                460, 529, 460, 442, 442, 484, 460, 440, 480, 453, 433, 459, 489, 710, 686, 379,
                494, 544, 563, 459, 543, 554, 570, 575, 554, 544, 527, 487, 496, 527, 540, 539,
                552, 548, 554, 576, 557, 507, 514, 395, 478, 484, 486, 470, 563, 558, 513, 503,
                526, 484, 490, 495, 563, 710, 684, 680, 689, 414, 391, 404, 409, 406, 419, 414,
                372, 409, 421, 455, 439, 478, 492, 481, 483, 465, 497, 502, 473, 476, 446, 500,
                478, 415, 645, 684, 683, 685, 672, 689, 690, 684, 698, 697, 720, 723, 762, 852,
                718, 660, 524, 700, 763, 757, 754, 661, 474, 432, 490, 478, 511, 507, 484, 499,
                507, 508, 497, 531, 500, 531, 531, 568, 522, 539, 507, 512, 529, 538, 526, 539,
                589, 600, 598, 629, 613, 538, 496, 387, 399, 375, 432, 403, 403, 366, 400, 419,
                435, 470, 511, 501, 496, 518, 536, 553, 512, 502, 486, 472, 431, 440
            };
            return read;
        }

        public List<PhenotypeData> GetPhenotypes()
        {
            return GetRandomPhenotypes();
        }

        public void ClearLists()
        {
            reads.Clear();
            phenotypeDatas.Clear();
        }

        public void ReceiveNewRead(ReadData read)
        {
            reads.Add(read);
        }

        public void ReceiveNewPhenotype(List<PhenotypeData> phenotypeData)
        {
            phenotypeDatas.AddRange(phenotypeData);
        }
    }
